using MediatR;
using Shared.Common.Application;

namespace CartService.Application.Commands.RemoveFromCart;

public record RemoveFromCartCommand : IRequest<Result>
{
    public int UserId { get; init; }
    public int ProductId { get; init; }
}
