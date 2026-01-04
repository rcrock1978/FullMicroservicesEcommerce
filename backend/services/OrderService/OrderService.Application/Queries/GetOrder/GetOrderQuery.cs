using OrderService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace OrderService.Application.Queries.GetOrder;

public record GetOrderQuery : IRequest<Result<OrderDto>>
{
    public int OrderId { get; init; }
}
