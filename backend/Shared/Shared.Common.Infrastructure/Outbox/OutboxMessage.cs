namespace Shared.Common.Infrastructure.Outbox;

/// <summary>
/// Outbox message for implementing the outbox pattern
/// Ensures reliable message publishing in distributed systems
/// </summary>
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
    public int RetryCount { get; set; }

    public bool IsProcessed => ProcessedOn.HasValue;
}
