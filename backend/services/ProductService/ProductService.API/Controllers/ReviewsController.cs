using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands.Reviews;
using ProductService.Contracts.DTOs;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
    {
        var command = new CreateReviewCommand(
            dto.ProductId,
            1, // TODO: Get from authenticated user
            dto.Rating,
            dto.Title,
            dto.Comment);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(CreateReview), new { id = result.Value!.Id }, result.Value);
    }
}
