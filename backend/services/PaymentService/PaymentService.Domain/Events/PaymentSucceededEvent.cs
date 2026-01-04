using Shared.Common.Domain;

namespace PaymentService.Domain.Events;

public class PaymentSucceededEvent : IDomainEvent
{
    public int PaymentId { get; }
    public int OrderId { get; }
    public int UserId { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public DateTime ProcessedAt { get; }
    public DateTime OccurredOn { get; }

    public PaymentSucceededEvent(int paymentId, int orderId, int userId, decimal amount, string currency, DateTime processedAt)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        UserId = userId;
        Amount = amount;
        Currency = currency;
        ProcessedAt = processedAt;
        OccurredOn = DateTime.UtcNow;
    }
}
