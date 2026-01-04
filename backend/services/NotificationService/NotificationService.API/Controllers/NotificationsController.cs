using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Commands;
using NotificationService.Application.Queries;
using NotificationService.Contracts.Dtos;
using System.Security.Claims;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotification(int id)
    {
        var query = new GetNotificationByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserNotifications()
    {
        var userId = GetUserId();
        var query = new GetUserNotificationsQuery(userId);
        var result = await _mediator.Send(query);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto dto)
    {
        var userId = GetUserId();
        var command = new SendNotificationCommand(userId, dto);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetNotification), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }
}
