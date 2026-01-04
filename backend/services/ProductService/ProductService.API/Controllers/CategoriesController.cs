using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands.Categories;
using ProductService.Application.Queries.Categories;
using ProductService.Contracts.DTOs;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetCategoriesQuery();
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var command = new CreateCategoryCommand(
            dto.Name,
            dto.Description,
            dto.Slug,
            dto.ParentCategoryId,
            dto.DisplayOrder);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetCategories), new { id = result.Value!.Id }, result.Value);
    }
}
