using Shared.Common.Domain;

namespace OrderService.Domain.Events;

public record OrderConfirmedEvent : IDomainEvent
{
    public int OrderId { get; init; }
    public string OrderNumber { get; init; }
    public DateTime OccurredOn { get; init; }

    public OrderConfirmedEvent(int orderId, string orderNumber)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        OccurredOn = DateTime.UtcNow;
    }
}
