namespace Shared.Messaging.Abstractions;

/// <summary>
/// Interface for publishing messages to the message bus
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes an event to the message bus
    /// </summary>
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IEvent;

    /// <summary>
    /// Publishes multiple events to the message bus
    /// </summary>
    Task PublishBatchAsync<T>(IEnumerable<T> messages, CancellationToken cancellationToken = default) where T : IEvent;
}
