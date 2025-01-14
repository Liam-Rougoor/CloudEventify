using System.Text.Json;
using Dapr.Client;

namespace CloudEventify.Dapr;

internal sealed class Client : DaprClient
{
    private readonly DaprClient _client;
    private readonly Wrap _wrap;

    public Client(DaprClient client, Wrap wrap)
    {
        _client = client;
        _wrap = wrap;
    }
    
    public override Task PublishEventAsync<TData>(string pubsubName, string topicName, TData data,
        CancellationToken cancellationToken = new()) =>
        _client.PublishEventAsync(pubsubName, topicName, _wrap.Envelope(data), cancellationToken);

    public override Task PublishEventAsync<TData>(string pubsubName, string topicName, TData data, Dictionary<string, string> metadata,
        CancellationToken cancellationToken = new()) =>
        _client.PublishEventAsync(pubsubName, topicName, _wrap.Envelope(data), metadata, cancellationToken);

    public override Task PublishEventAsync(string pubsubName, string topicName, CancellationToken cancellationToken = new()) =>
        _client.PublishEventAsync(pubsubName, topicName, cancellationToken);

    public override Task PublishEventAsync(string pubsubName, string topicName, Dictionary<string, string> metadata, CancellationToken cancellationToken = new()) =>
        _client.PublishEventAsync(pubsubName, topicName, metadata, cancellationToken);

    public override Task InvokeBindingAsync<TRequest>(string bindingName, string operation, TRequest data, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.InvokeBindingAsync(bindingName, operation, _wrap.Envelope(data), metadata, cancellationToken);

    public override Task<TResponse> InvokeBindingAsync<TRequest, TResponse>(string bindingName, string operation, TRequest data, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.InvokeBindingAsync<TRequest, TResponse>(bindingName, operation, data, metadata, cancellationToken);

    public override Task<BindingResponse> InvokeBindingAsync(BindingRequest request, CancellationToken cancellationToken = new()) => 
        _client.InvokeBindingAsync(request, cancellationToken);

    public override HttpRequestMessage CreateInvokeMethodRequest(HttpMethod httpMethod, string appId, string methodName) => 
        _client.CreateInvokeMethodRequest(httpMethod, appId, methodName);

    public override HttpRequestMessage CreateInvokeMethodRequest<TRequest>(HttpMethod httpMethod, string appId, string methodName, TRequest data) =>
        _client.CreateInvokeMethodRequest(httpMethod, appId, methodName, data);

    public override Task<bool> CheckHealthAsync(CancellationToken cancellationToken = new()) => 
        _client.CheckHealthAsync(cancellationToken);

    public override Task<bool> CheckOutboundHealthAsync(CancellationToken cancellationToken = new()) => 
        _client.CheckOutboundHealthAsync(cancellationToken);

    public override Task WaitForSidecarAsync(CancellationToken cancellationToken = new()) => 
        _client.WaitForSidecarAsync(cancellationToken);

    public override Task<HttpResponseMessage> InvokeMethodWithResponseAsync(HttpRequestMessage request, CancellationToken cancellationToken = new()) =>
        _client.InvokeMethodWithResponseAsync(request, cancellationToken);

    public override Task InvokeMethodAsync(HttpRequestMessage request, CancellationToken cancellationToken = new()) => 
        _client.InvokeMethodAsync(request, cancellationToken);

    public override Task<TResponse> InvokeMethodAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = new()) =>
        _client.InvokeMethodAsync<TResponse>(request, cancellationToken);

    public override Task InvokeMethodGrpcAsync(string appId, string methodName, CancellationToken cancellationToken = new()) =>
        _client.InvokeMethodGrpcAsync(appId, methodName, cancellationToken);

    public override Task InvokeMethodGrpcAsync<TRequest>(string appId, string methodName, TRequest data, CancellationToken cancellationToken = new()) =>
        _client.InvokeMethodGrpcAsync(appId, methodName, data, cancellationToken);

    public override Task<TResponse> InvokeMethodGrpcAsync<TResponse>(string appId, string methodName, CancellationToken cancellationToken = new()) =>
        _client.InvokeMethodGrpcAsync<TResponse>(appId, methodName, cancellationToken);

    public override Task<TResponse> InvokeMethodGrpcAsync<TRequest, TResponse>(string appId, string methodName, TRequest data, CancellationToken cancellationToken = new()) =>
        _client.InvokeMethodGrpcAsync<TRequest, TResponse>(appId, methodName, data, cancellationToken);

    public override Task<TValue> GetStateAsync<TValue>(string storeName, string key, ConsistencyMode? consistencyMode = null, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.GetStateAsync<TValue>(storeName, key, consistencyMode, metadata, cancellationToken);

    public override Task<IReadOnlyList<BulkStateItem>> GetBulkStateAsync(string storeName, IReadOnlyList<string> keys, int? parallelism, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.GetBulkStateAsync(storeName, keys, parallelism, metadata, cancellationToken);

    public override Task DeleteBulkStateAsync(string storeName, IReadOnlyList<BulkDeleteStateItem> items, CancellationToken cancellationToken = new()) =>
        _client.DeleteBulkStateAsync(storeName, items, cancellationToken);

    public override Task<(TValue value, string etag)> GetStateAndETagAsync<TValue>(string storeName, string key, ConsistencyMode? consistencyMode = null, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.GetStateAndETagAsync<TValue>(storeName, key, consistencyMode, metadata, cancellationToken);

    public override Task SaveStateAsync<TValue>(string storeName, string key, TValue value, StateOptions stateOptions = null, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.SaveStateAsync(storeName, key, value, stateOptions, metadata, cancellationToken);

    public override Task<bool> TrySaveStateAsync<TValue>(string storeName, string key, TValue value, string etag, StateOptions stateOptions = null, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.TrySaveStateAsync(storeName, key, value, etag, stateOptions, metadata, cancellationToken);

    public override Task ExecuteStateTransactionAsync(string storeName, IReadOnlyList<StateTransactionRequest> operations, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.ExecuteStateTransactionAsync(storeName, operations, metadata, cancellationToken);

    public override Task DeleteStateAsync(string storeName, string key, StateOptions stateOptions = null, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.DeleteStateAsync(storeName, key, stateOptions, metadata, cancellationToken);

    public override Task<bool> TryDeleteStateAsync(string storeName, string key, string etag, StateOptions stateOptions = null, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.TryDeleteStateAsync(storeName, key, etag, stateOptions, metadata, cancellationToken);

    public override Task<StateQueryResponse<TValue>> QueryStateAsync<TValue>(string storeName, string jsonQuery, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.QueryStateAsync<TValue>(storeName, jsonQuery, metadata, cancellationToken);

    public override Task<Dictionary<string, string>> GetSecretAsync(string storeName, string key, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.GetSecretAsync(storeName, key, metadata, cancellationToken);

    public override Task<Dictionary<string, Dictionary<string, string>>> GetBulkSecretAsync(string storeName, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.GetBulkSecretAsync(storeName, metadata, cancellationToken);

    public override Task<GetConfigurationResponse> GetConfiguration(string storeName, IReadOnlyList<string> keys, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.GetConfiguration(storeName, keys, metadata, cancellationToken);

    public override Task<SubscribeConfigurationResponse> SubscribeConfiguration(string storeName, IReadOnlyList<string> keys, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = new()) =>
        _client.SubscribeConfiguration(storeName, keys, metadata, cancellationToken);

    public override Task<UnsubscribeConfigurationResponse> UnsubscribeConfiguration(string storeName, string id, CancellationToken cancellationToken = new()) =>
        _client.UnsubscribeConfiguration(storeName, id, cancellationToken);

    public override Task<TryLockResponse> Lock(string storeName, string resourceId, string lockOwner, int expiryInSeconds, CancellationToken cancellationToken = new()) =>
        _client.Lock(storeName, resourceId, lockOwner, expiryInSeconds, cancellationToken);

    public override Task<UnlockResponse> Unlock(string storeName, string resourceId, string lockOwner, CancellationToken cancellationToken = new()) =>
        _client.Unlock(storeName, resourceId, lockOwner, cancellationToken);

    public override JsonSerializerOptions JsonSerializerOptions => 
        _client.JsonSerializerOptions;

    protected override void Dispose(bool disposing)
    {
        _client.Dispose();
        base.Dispose(disposing);
    }
}