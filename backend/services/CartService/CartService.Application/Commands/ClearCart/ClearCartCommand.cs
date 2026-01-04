using MediatR;
using Shared.Common.Application;

namespace CartService.Application.Commands.ClearCart;

public record ClearCartCommand : IRequest<Result>
{
    public int UserId { get; init; }
}
