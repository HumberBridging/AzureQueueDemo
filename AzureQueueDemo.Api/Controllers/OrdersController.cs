using AzureQueueDemo.Api.Dto;
using AzureQueueDemo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureQueueDemo.Api.Controllers;

[ApiController]
[Route("api/v1/orders")]
[Produces("application/json")]
public sealed class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderPublisher _orderPublisher;

    public OrdersController(ILogger<OrdersController> logger, IOrderPublisher orderPublisher)
    {
        _logger = logger;
        _orderPublisher = orderPublisher;
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest order, CancellationToken cancellationToken)
    {
        if (order == null)
        {
            return BadRequest("Order cannot be null.");
        }

        var messageId = await _orderPublisher.PublishAsync(order, cancellationToken);

        _logger.LogInformation("Order {OrderId} placed and published to queue with message id {MessageId}", order.OrderId, messageId);

        return Accepted(new { OrderId = order.OrderId, MessageId = messageId });
    }
}
