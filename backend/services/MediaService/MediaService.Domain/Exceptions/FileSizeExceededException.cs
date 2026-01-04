using Shared.Common.Domain.Exceptions;

namespace MediaService.Domain.Exceptions;

public class FileSizeExceededException : DomainValidationException
{
    public FileSizeExceededException(long fileSize, long maxSize)
        : base($"File size ({fileSize} bytes) exceeds maximum allowed size ({maxSize} bytes)")
    {
    }
}
