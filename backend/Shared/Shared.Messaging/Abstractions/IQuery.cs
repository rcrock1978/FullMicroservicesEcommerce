namespace Shared.Messaging.Abstractions;

/// <summary>
/// Interface for queries - messages that request data without side effects
/// Queries follow CQRS pattern for read operations
/// </summary>
/// <typeparam name="TResponse">The type of response expected from the query</typeparam>
public interface IQuery<out TResponse> : IMessage
{
}
