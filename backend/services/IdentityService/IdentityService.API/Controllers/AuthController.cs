using IdentityService.Application.Commands.Auth;
using IdentityService.Application.Commands.Users;
using IdentityService.Contracts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to register user with email: {Email}", dto.Email);
        
        var command = new RegisterUserCommand(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.Password,
            dto.PhoneNumber
        );
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        _logger.LogInformation("User registered successfully with ID: {UserId}", result.Value.Id);
        return CreatedAtAction(nameof(Register), new { id = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for email: {Email}", dto.Email);
        
        var command = new LoginCommand(dto.Email, dto.Password);
        var token = await _mediator.Send(command, cancellationToken);
        
        _logger.LogInformation("User logged in successfully: {Email}", dto.Email);
        return Ok(token);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] string refreshToken, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to refresh token");
        
        var command = new RefreshTokenCommand(refreshToken);
        var token = await _mediator.Send(command, cancellationToken);
        
        _logger.LogInformation("Token refreshed successfully");
        return Ok(token);
    }

    /// <summary>
    /// Verify user email
    /// </summary>
    [HttpPost("verify-email")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> VerifyEmail([FromBody] string verificationToken, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        _logger.LogInformation("Attempting to verify email for user: {UserId}", userId);
        
        var command = new VerifyEmailCommand(int.Parse(userId), verificationToken);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        _logger.LogInformation("Email verified successfully for user: {UserId}", userId);
        return Ok(new { message = "Email verified successfully" });
    }

    /// <summary>
    /// Change user password
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        _logger.LogInformation("Attempting to change password for user: {UserId}", userId);
        
        var command = new ChangePasswordCommand(int.Parse(userId), dto.CurrentPassword, dto.NewPassword);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        
        _logger.LogInformation("Password changed successfully for user: {UserId}", userId);
        return Ok(new { message = "Password changed successfully" });
    }
}
