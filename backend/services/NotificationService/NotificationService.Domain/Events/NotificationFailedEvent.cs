using NotificationService.Domain.Enums;
using Shared.Common.Domain;

namespace NotificationService.Domain.Events;

public record NotificationFailedEvent(
    int NotificationId,
    int UserId,
    NotificationType Type,
    string FailureReason) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
