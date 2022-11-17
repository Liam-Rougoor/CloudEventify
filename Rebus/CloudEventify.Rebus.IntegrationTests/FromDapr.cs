using System;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using CloudEventify.Dapr;
using FluentAssertions.Extensions;
using Hypothesist;
using Rebus.Activation;
using Rebus.Config;
using Wrapr;
using Xunit;
using Xunit.Abstractions;
using Dapr.Client;
using RabbitMQ.Client;
using Rebus.Pipeline;
using Headers = Rebus.Messages.Headers;

namespace CloudEventify.Rebus.IntegrationTests;

[Collection("user/loggedIn")]
public class FromDapr : IClassFixture<RabbitMqContainer>
{
    private readonly ITestOutputHelper _output;
    private readonly RabbitMqContainer _container;

    public FromDapr(ITestOutputHelper output, RabbitMqContainer container)
    {
        _output = output;
        _container = container;
    }

    [Fact]
    public async Task Do()
    {
        // Arrange
        const string topic = "user/loggedIn";
        const string queue = "user:loggedIn:rabbitmq";
        
        var message = Message();
        var hypothesis = Hypothesis
            .For<(IMessageContext c, UserLoggedIn m)>()
            .Any(x => x.m == message)
            .Any(x => x.c.Headers[Headers.SenderAddress] == "cloudeventify:dapr")
            .Any(x => x.c.Headers.ContainsKey(Headers.CorrelationId));

        var activator = new BuiltinHandlerActivator()
            .Handle<UserLoggedIn>(async (_, c, m) => await hypothesis.Test((c, m)));
        var subscriber = Configure.With(activator)
            .Transport(t => t.UseRabbitMq(_container.ConnectionString, queue))
            .Serialization(s => s.UseCloudEvents(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                .AddWithCustomName<UserLoggedIn>("loggedIn"))
            .Options(o => o.InjectMessageId()) // <-- currently not req'd for RabbitMQ but want to include it in the test anyway
            .Options(o => o.UseCustomTypeNameForTopicName())
            .Logging(l => l.MicrosoftExtensionsLogging(_output.ToLoggerFactory()))
            .Start();
        await subscriber.Subscribe<UserLoggedIn>();

        BindRebus(_container.ConnectionString, topic, queue);
        
        // Act
        await Publish(topic, message, _output);

        // Assert
        await hypothesis.Validate(5.Seconds());
    }
    
    private static void BindRebus(string connectionString, string topic, string queue)
    {
        var model = new ConnectionFactory
            {
                Endpoint = new AmqpTcpEndpoint(new Uri(connectionString))
            }
            .CreateConnection()
            .CreateModel();
        
        model.ExchangeDeclare(topic, "fanout", durable: true);
        model.QueueBind(queue, topic, "");
    }

    private static async Task Publish(string topic, UserLoggedIn message, ITestOutputHelper logger)
    {
        await using var sidecar = new Sidecar("from-dapr-to-rabbitmq", logger.ToLogger<Sidecar>());
        await sidecar.Start(with => with
            .ComponentsPath("components")
            .DaprGrpcPort(3201));

        using var client = new DaprClientBuilder()
            .UseGrpcEndpoint("http://localhost:3201")
            .UseCloudEvents()
            .WithTypes(types => types.Map<UserLoggedIn>("loggedIn"))
            .Build();
        
        await client
            .PublishEventAsync("my-pubsub", topic, message);
    }

    public record UserLoggedIn(string Id);
        
    private static UserLoggedIn Message() => 
        new Faker<UserLoggedIn>()
            .CustomInstantiator(f => new UserLoggedIn(f.Random.Hash()))
            .Generate();
}