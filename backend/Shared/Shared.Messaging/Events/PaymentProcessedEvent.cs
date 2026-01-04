using Shared.Messaging.Abstractions;

namespace Shared.Messaging.Events;

/// <summary>
/// Event raised when a payment has been processed
/// </summary>
public class PaymentProcessedEvent : IEvent
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string? CorrelationId { get; init; }
    public string EventType => nameof(PaymentProcessedEvent);

    public int PaymentId { get; init; }
    public int OrderId { get; init; }
    public int UserId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";
    public string PaymentMethod { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? TransactionId { get; init; }
    public DateTime ProcessedAt { get; init; }
}
