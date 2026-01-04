using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Commands;
using PaymentService.Application.Queries;
using PaymentService.Contracts.Dtos;
using System.Security.Claims;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayment(int id)
    {
        var query = new GetPaymentByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetPaymentByOrder(int orderId)
    {
        var query = new GetPaymentByOrderIdQuery(orderId);
        var result = await _mediator.Send(query);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserPayments()
    {
        var userId = GetUserId();
        var query = new GetUserPaymentsQuery(userId);
        var result = await _mediator.Send(query);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("initiate")]
    public async Task<IActionResult> InitiatePayment([FromBody] CreatePaymentDto dto)
    {
        var userId = GetUserId();
        var command = new InitiatePaymentCommand(userId, dto);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetPayment), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmPayment(int id)
    {
        var command = new ConfirmPaymentCommand(id);
        var result = await _mediator.Send(command);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("{id}/refund")]
    public async Task<IActionResult> RefundPayment(int id, [FromBody] RefundPaymentDto dto)
    {
        var command = new RefundPaymentCommand(id, dto);
        var result = await _mediator.Send(command);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }
}
