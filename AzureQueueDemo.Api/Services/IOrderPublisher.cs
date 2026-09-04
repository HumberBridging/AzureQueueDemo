using AzureQueueDemo.Api.Dto;

namespace AzureQueueDemo.Api.Services;

public interface IOrderPublisher
{
    Task<string> PublishAsync(PlaceOrderRequest order, CancellationToken cancellationToken = default);
}
