namespace Shared.Common.Application.Interfaces;

/// <summary>
/// Interface for date/time abstraction (useful for testing)
/// </summary>
public interface IDateTime
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTimeOffset NowOffset { get; }
    DateTimeOffset UtcNowOffset { get; }
}

/// <summary>
/// Default implementation of IDateTime
/// </summary>
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset NowOffset => DateTimeOffset.Now;
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}
