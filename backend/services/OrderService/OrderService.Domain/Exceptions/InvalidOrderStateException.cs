using Shared.Common.Domain.Exceptions;

namespace OrderService.Domain.Exceptions;

public class InvalidOrderStateException : DomainValidationException
{
    public InvalidOrderStateException(string message)
        : base(message)
    {
    }
}
