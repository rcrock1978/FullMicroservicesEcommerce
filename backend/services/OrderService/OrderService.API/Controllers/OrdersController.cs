using OrderService.Application.Commands.CancelOrder;
using OrderService.Application.Commands.CreateOrder;
using OrderService.Application.Commands.UpdateOrderStatus;
using OrderService.Application.Queries.GetOrder;
using OrderService.Application.Queries.GetUserOrders;
using OrderService.Contracts.DTOs;
using OrderService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id, CancellationToken cancellationToken)
    {
        var query = new GetOrderQuery { OrderId = id };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserOrders(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var query = new GetUserOrdersQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new CreateOrderCommand
        {
            UserId = userId,
            Items = dto.Items,
            ShippingAddress = dto.ShippingAddress,
            PaymentMethod = dto.PaymentMethod,
            ShippingCost = dto.ShippingCost,
            Notes = dto.Notes
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetOrder), new { id = result.Value!.Id }, result.Value);

        return BadRequest(result.Error);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(
        int id,
        [FromBody] UpdateOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            NewStatus = request.Status
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result.Error);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(
        int id,
        [FromBody] CancelOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CancelOrderCommand
        {
            OrderId = id,
            Reason = request.Reason
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result.Error);
    }
}

public record UpdateOrderStatusRequest(OrderStatus Status);
public record CancelOrderRequest(string Reason);
