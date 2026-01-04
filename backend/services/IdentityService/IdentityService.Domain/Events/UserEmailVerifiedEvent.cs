using Shared.Common.Domain;

namespace IdentityService.Domain.Events;

public record UserEmailVerifiedEvent(
    int UserId,
    string Email,
    DateTime VerifiedAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
