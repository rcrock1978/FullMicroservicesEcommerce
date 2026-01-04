namespace Shared.Messaging.Abstractions;

/// <summary>
/// Base interface for all messages in the system
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Unique identifier for the message
    /// </summary>
    Guid MessageId { get; }

    /// <summary>
    /// Timestamp when the message was created
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Correlation identifier for tracking related messages
    /// </summary>
    string? CorrelationId { get; }
}
