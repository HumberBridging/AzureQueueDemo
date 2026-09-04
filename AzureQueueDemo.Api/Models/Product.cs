namespace AzureQueueDemo.Api.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}
