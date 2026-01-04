using Shared.Common.Domain;

namespace OrderService.Domain.Events;

public record OrderCancelledEvent : IDomainEvent
{
    public int OrderId { get; init; }
    public string OrderNumber { get; init; }
    public string Reason { get; init; }
    public DateTime OccurredOn { get; init; }

    public OrderCancelledEvent(int orderId, string orderNumber, string reason)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        Reason = reason;
        OccurredOn = DateTime.UtcNow;
    }
}
