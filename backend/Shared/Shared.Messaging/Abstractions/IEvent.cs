namespace Shared.Messaging.Abstractions;

/// <summary>
/// Interface for events - messages that represent something that has happened
/// Events are declarative and can have multiple handlers
/// </summary>
public interface IEvent : IMessage
{
    /// <summary>
    /// The name of the event type
    /// </summary>
    string EventType { get; }
}
