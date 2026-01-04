using Shared.Common.Domain;

namespace OrderService.Domain.Events;

public record OrderCreatedEvent : IDomainEvent
{
    public int OrderId { get; init; }
    public int UserId { get; init; }
    public string OrderNumber { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime OccurredOn { get; init; }

    public OrderCreatedEvent(int orderId, int userId, string orderNumber, decimal totalAmount)
    {
        OrderId = orderId;
        UserId = userId;
        OrderNumber = orderNumber;
        TotalAmount = totalAmount;
        OccurredOn = DateTime.UtcNow;
    }
}
