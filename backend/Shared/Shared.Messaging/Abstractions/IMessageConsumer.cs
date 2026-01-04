namespace Shared.Messaging.Abstractions;

/// <summary>
/// Interface for consuming messages from the message bus
/// </summary>
public interface IMessageConsumer
{
    /// <summary>
    /// Starts consuming messages of the specified type
    /// </summary>
    Task StartAsync<T>(Func<T, CancellationToken, Task> handler, CancellationToken cancellationToken = default) where T : IEvent;

    /// <summary>
    /// Stops consuming messages
    /// </summary>
    Task StopAsync(CancellationToken cancellationToken = default);
}
