using CartService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace CartService.Application.Commands.UpdateCartItem;

public record UpdateCartItemCommand : IRequest<Result<CartDto>>
{
    public int UserId { get; init; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
