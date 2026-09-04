using Azure.Storage.Queues;
using AzureQueueDemo.Api.Dto;
using AzureQueueDemo.Api.Options;
using Microsoft.Extensions.Options;

namespace AzureQueueDemo.Api.Services;

public class OrderPublisher : IOrderPublisher
{
    private readonly ILogger<OrderPublisher> _logger;
    private readonly QueueStorageOptions _storageOptions;

    public OrderPublisher(ILogger<OrderPublisher> logger, IOptions<QueueStorageOptions> options)
    {
        _logger = logger;
        _storageOptions = options.Value;
    }

    public async Task<string> PublishAsync(PlaceOrderRequest order, CancellationToken cancellationToken = default)
    {
        //Observe how inefficient this is, we are creating a new QueueClient for every message we send. This is not a good practice, but it is done here for demonstration purposes.
        var client = new QueueClient(_storageOptions.ConnectionString, _storageOptions.OrdersQueueName, new QueueClientOptions()
        {
            MessageEncoding = QueueMessageEncoding.Base64 //or you can use none, but base64 is safer for binary data
        });

        //you need to seralize the order object to a string before sending it to the queue. You can use any serialization method you prefer, such as JSON or XML. Here, we will use System.Text.Json for simplicity.
        var orderJson = System.Text.Json.JsonSerializer.Serialize(order);

        var response = await client.SendMessageAsync(messageText: orderJson,
            // visibilityTimeout: how long before a consumer can first SEE this message.
            // Non-zero turns this into a scheduled/delayed message - useful for retries and
            // "send the reminder in 10 minutes" workflows.
            visibilityTimeout: TimeSpan.Zero,

            // timeToLive: after this, Azure silently deletes the message. Default is 7 days.
            // TimeSpan.FromSeconds(-1) means "never expire". Choose deliberately: an infinite TTL
            // on a queue nobody drains is a slow-motion incident.
            timeToLive: TimeSpan.FromDays(2),

            cancellationToken: cancellationToken);

        _logger.LogInformation("Published order {OrderId} to queue {QueueName} with message id {MessageId}", order.OrderId, _storageOptions.OrdersQueueName, response.Value.MessageId);
        
        return response.Value.MessageId;
    }
}
