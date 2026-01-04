using OrderService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace OrderService.Application.Queries.GetUserOrders;

public record GetUserOrdersQuery : IRequest<Result<List<OrderDto>>>
{
    public int UserId { get; init; }
}
