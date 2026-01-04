using Shared.Messaging.Abstractions;

namespace Shared.Messaging.Events;

/// <summary>
/// Event raised when product stock quantity changes
/// </summary>
public class ProductStockChangedEvent : IEvent
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string? CorrelationId { get; init; }
    public string EventType => nameof(ProductStockChangedEvent);

    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int PreviousQuantity { get; init; }
    public int NewQuantity { get; init; }
    public int QuantityChanged { get; init; }
    public string ChangeReason { get; init; } = string.Empty;
    public DateTime ChangedAt { get; init; }
}
