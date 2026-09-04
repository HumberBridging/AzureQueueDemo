namespace AzureQueueDemo.Api.Models;

public class OrderLineItem
{
    public Guid ProductId { get; set; }
    public int Units { get; init; }
    public decimal UnitPrice { get; set; } = 0;
}

