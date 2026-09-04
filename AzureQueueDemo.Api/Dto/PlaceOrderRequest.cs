using AzureQueueDemo.Api.Models;

namespace AzureQueueDemo.Api.Dto;

public sealed record PlaceOrderRequest
{
    public required Guid OrderId { get; init; }
    public required int CustomerId { get; init; }
    public List<OrderLineItem> Items { get; set; } = new();
}
