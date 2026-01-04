namespace Shared.Common.Domain.Exceptions;

/// <summary>
/// Exception thrown when domain validation fails
/// </summary>
public class DomainValidationException : Exception
{
    public List<string> Errors { get; }

    public DomainValidationException(string message)
        : base(message)
    {
        Errors = new List<string> { message };
    }

    public DomainValidationException(List<string> errors)
        : base("One or more validation errors occurred")
    {
        Errors = errors;
    }

    public DomainValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = new List<string> { message };
    }
}
