using Shared.Messaging.Abstractions;

namespace Shared.Messaging.Events;

/// <summary>
/// Event raised when a new order is created
/// </summary>
public class OrderCreatedEvent : IEvent
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string? CorrelationId { get; init; }
    public string EventType => nameof(OrderCreatedEvent);

    public int OrderId { get; init; }
    public int UserId { get; init; }
    public decimal TotalAmount { get; init; }
    public string Currency { get; init; } = "USD";
    public List<OrderItemDto> Items { get; init; } = new();
    public DateTime OrderedAt { get; init; }
}

public class OrderItemDto
{
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
