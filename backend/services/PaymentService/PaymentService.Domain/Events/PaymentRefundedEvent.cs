using Shared.Common.Domain;

namespace PaymentService.Domain.Events;

public record PaymentRefundedEvent(
    int PaymentId,
    int UserId,
    int OrderId,
    decimal RefundAmount,
    DateTime RefundedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
