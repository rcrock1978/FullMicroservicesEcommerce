using MediatR;
using Shared.Common.Application;

namespace OrderService.Application.Commands.CancelOrder;

public record CancelOrderCommand : IRequest<Result>
{
    public int OrderId { get; init; }
    public string Reason { get; init; } = string.Empty;
}
