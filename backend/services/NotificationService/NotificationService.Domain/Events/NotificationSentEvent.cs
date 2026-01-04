using NotificationService.Domain.Enums;
using Shared.Common.Domain;

namespace NotificationService.Domain.Events;

public record NotificationSentEvent(
    int NotificationId,
    int UserId,
    NotificationType Type,
    DateTime SentAt) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
