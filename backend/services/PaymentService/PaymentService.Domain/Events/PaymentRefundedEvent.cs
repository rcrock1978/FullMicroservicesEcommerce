using Shared.Common.Domain;

namespace PaymentService.Domain.Events;

public class PaymentRefundedEvent : IDomainEvent
{
    public int PaymentId { get; }
    public int OrderId { get; }
    public decimal RefundedAmount { get; }
    public string RefundReason { get; }
    public DateTime RefundedAt { get; }
    public DateTime OccurredOn { get; }

    public PaymentRefundedEvent(int paymentId, int orderId, decimal refundedAmount, string refundReason, DateTime refundedAt)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        RefundedAmount = refundedAmount;
        RefundReason = refundReason;
        RefundedAt = refundedAt;
        OccurredOn = DateTime.UtcNow;
    }
}
