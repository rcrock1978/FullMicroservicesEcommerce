using Shared.Common.Domain;

namespace PaymentService.Domain.Events;

public class PaymentInitiatedEvent : IDomainEvent
{
    public int PaymentId { get; }
    public int OrderId { get; }
    public int UserId { get; }
    public decimal Amount { get; }
    public string Currency { get; }
    public DateTime OccurredOn { get; }

    public PaymentInitiatedEvent(int paymentId, int orderId, int userId, decimal amount, string currency)
    {
        PaymentId = paymentId;
        OrderId = orderId;
        UserId = userId;
        Amount = amount;
        Currency = currency;
        OccurredOn = DateTime.UtcNow;
    }
}
