using Shared.Common.Domain;

namespace PaymentService.Domain.Events;

public record PaymentCompletedEvent(
    int PaymentId,
    int UserId,
    int OrderId,
    decimal Amount,
    DateTime CompletedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
