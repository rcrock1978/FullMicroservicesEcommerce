using NotificationService.Domain.Enums;
using Shared.Common.Domain;

namespace NotificationService.Domain.Events;

public record NotificationCreatedEvent(
    int NotificationId,
    int UserId,
    NotificationType Type,
    string Subject) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
