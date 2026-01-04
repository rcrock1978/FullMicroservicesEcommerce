using Shared.Common.Domain;

namespace PaymentService.Domain.Events;

public record PaymentFailedEvent(
    int PaymentId,
    int UserId,
    int OrderId,
    string FailureReason) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
