using Shared.Common.Domain;

namespace IdentityService.Domain.Events;

public record UserRegisteredEvent(
    int UserId,
    string Email,
    string FirstName,
    string LastName,
    DateTime RegisteredAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
