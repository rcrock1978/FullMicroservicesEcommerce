using CartService.Application.Commands.AddToCart;
using CartService.Application.Commands.ClearCart;
using CartService.Application.Commands.RemoveFromCart;
using CartService.Application.Commands.UpdateCartItem;
using CartService.Application.Queries.GetCart;
using CartService.Contracts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var query = new GetCartQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new AddToCartCommand
        {
            UserId = userId,
            ProductId = dto.ProductId,
            ProductName = dto.ProductName,
            UnitPrice = dto.UnitPrice,
            Quantity = dto.Quantity,
            ImageUrl = dto.ImageUrl
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [HttpPut("items")]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new UpdateCartItemCommand
        {
            UserId = userId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [HttpDelete("items/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int productId, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new RemoveFromCartCommand
        {
            UserId = userId,
            ProductId = productId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result.Error);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var command = new ClearCartCommand { UserId = userId };
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result.Error);
    }
}
