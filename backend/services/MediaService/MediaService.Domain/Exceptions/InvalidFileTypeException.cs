using Shared.Common.Domain.Exceptions;

namespace MediaService.Domain.Exceptions;

public class InvalidFileTypeException : DomainValidationException
{
    public InvalidFileTypeException(string contentType)
        : base($"File type '{contentType}' is not supported")
    {
    }

    public InvalidFileTypeException(string contentType, string[] allowedTypes)
        : base($"File type '{contentType}' is not supported. Allowed types: {string.Join(", ", allowedTypes)}")
    {
    }
}
