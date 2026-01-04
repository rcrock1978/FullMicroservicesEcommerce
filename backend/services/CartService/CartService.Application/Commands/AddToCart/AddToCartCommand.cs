using CartService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace CartService.Application.Commands.AddToCart;

public record AddToCartCommand : IRequest<Result<CartDto>>
{
    public int UserId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; } = 1;
    public string? ImageUrl { get; init; }
}
