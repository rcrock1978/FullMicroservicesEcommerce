using Shared.Messaging.Abstractions;

namespace Shared.Messaging.Events;

/// <summary>
/// Event raised when a new user is created in the system
/// </summary>
public class UserCreatedEvent : IEvent
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string? CorrelationId { get; init; }
    public string EventType => nameof(UserCreatedEvent);

    public int UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime RegisteredAt { get; init; }
}
