using Shared.Common.Domain;

namespace PaymentService.Domain.Events;

public class PaymentFailedEvent : IDomainEvent
{
    public int PaymentId { get; }
    public int OrderId { get; }
    public int UserId { get; }
    public string FailureReason { get; }
    public DateTime OccurredOn { get; }

    public PaymentFailedEvent(int paymentId, int orderId, int userId, string failureReason)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        UserId = userId;
        FailureReason = failureReason;
        OccurredOn = DateTime.UtcNow;
    }
}
