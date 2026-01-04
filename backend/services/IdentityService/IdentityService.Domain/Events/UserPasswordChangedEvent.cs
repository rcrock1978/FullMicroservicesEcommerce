using Shared.Common.Domain;

namespace IdentityService.Domain.Events;

public record UserPasswordChangedEvent(
    int UserId,
    string Email,
    DateTime ChangedAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
