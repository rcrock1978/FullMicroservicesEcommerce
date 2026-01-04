using CartService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace CartService.Application.Queries.GetCart;

public record GetCartQuery : IRequest<Result<CartDto>>
{
    public int UserId { get; init; }
}
