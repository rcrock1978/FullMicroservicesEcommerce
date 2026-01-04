using Shared.Common.Domain;

namespace OrderService.Domain.Events;

public record OrderShippedEvent : IDomainEvent
{
    public int OrderId { get; init; }
    public string OrderNumber { get; init; }
    public DateTime ShippedAt { get; init; }
    public DateTime OccurredOn { get; init; }

    public OrderShippedEvent(int orderId, string orderNumber, DateTime shippedAt)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        ShippedAt = shippedAt;
        OccurredOn = DateTime.UtcNow;
    }
}
