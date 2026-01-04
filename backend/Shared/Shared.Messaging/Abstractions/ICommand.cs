namespace Shared.Messaging.Abstractions;

/// <summary>
/// Interface for commands - messages that represent an action to be performed
/// Commands are imperative and expect a single handler
/// </summary>
public interface ICommand : IMessage
{
}

/// <summary>
/// Generic command interface with a return type
/// </summary>
/// <typeparam name="TResponse">The type of response expected from the command</typeparam>
public interface ICommand<out TResponse> : IMessage
{
}
