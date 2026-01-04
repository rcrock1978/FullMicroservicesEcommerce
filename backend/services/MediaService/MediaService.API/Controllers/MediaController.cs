using MediatR;
using MediaService.Application.Commands;
using MediaService.Application.Queries;
using MediaService.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MediaController> _logger;

    public MediaController(IMediator mediator, ILogger<MediaController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("upload")]
    [Authorize]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileDto dto, CancellationToken cancellationToken)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("File is required");

        var userId = GetUserId();
        
        using var stream = dto.File.OpenReadStream();
        var command = new UploadFileCommand(
            stream,
            dto.File.FileName,
            dto.File.ContentType,
            dto.File.Length,
            userId,
            dto.MediaType,
            dto.EntityId,
            dto.EntityType
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileById(int id, CancellationToken cancellationToken)
    {
        var query = new GetFileByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    [HttpGet("entity/{entityId}/{entityType}")]
    public async Task<IActionResult> GetFilesByEntity(int entityId, string entityType, CancellationToken cancellationToken)
    {
        var query = new GetFilesByEntityQuery(entityId, entityType);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteFile(int id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new DeleteFileCommand(id, userId);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result.Error);
    }

    [HttpPost("{id}/thumbnail")]
    [Authorize]
    public async Task<IActionResult> GenerateThumbnail(
        int id,
        [FromBody] GenerateThumbnailDto dto,
        CancellationToken cancellationToken)
    {
        var command = new GenerateThumbnailCommand(
            id,
            dto.Width,
            dto.Height,
            dto.MaintainAspectRatio
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("userId");
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            return userId;

        return 0; // Default for unauthenticated users
    }
}
