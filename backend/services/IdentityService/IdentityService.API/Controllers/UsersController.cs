using IdentityService.Application.Queries.Users;
using IdentityService.Contracts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching user with ID: {UserId}", id);
        
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess || result.Value == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
            return NotFound(new { message = $"User with ID {id} not found" });
        }
        
        return Ok(result.Value);
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    [HttpGet("by-email/{email}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GetByEmail(string email, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching user with email: {Email}", email);
        
        var query = new GetUserByEmailQuery(email);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess || result.Value == null)
        {
            _logger.LogWarning("User not found with email: {Email}", email);
            return NotFound(new { message = $"User with email {email} not found" });
        }
        
        return Ok(result.Value);
    }

    /// <summary>
    /// Get current authenticated user's profile
    /// </summary>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GetProfile(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        _logger.LogInformation("Fetching profile for user: {UserId}", userId);
        
        var query = new GetUserByIdQuery(int.Parse(userId));
        var result = await _mediator.Send(query, cancellationToken);
        
        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound(new { message = "User profile not found" });
        }
        
        return Ok(result.Value);
    }
}
