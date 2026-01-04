namespace Shared.Common.Infrastructure.Outbox;

/// <summary>
/// Interface for outbox message processor
/// Processes unprocessed outbox messages
/// </summary>
public interface IOutboxMessageProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken = default);
}
