using OrderService.Domain.Enums;
using MediatR;
using Shared.Common.Application;

namespace OrderService.Application.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand : IRequest<Result>
{
    public int OrderId { get; init; }
    public OrderStatus NewStatus { get; init; }
}
