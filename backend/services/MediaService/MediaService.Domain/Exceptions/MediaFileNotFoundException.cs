using Shared.Common.Domain.Exceptions;

namespace MediaService.Domain.Exceptions;

public class MediaFileNotFoundException : NotFoundException
{
    public MediaFileNotFoundException(int mediaFileId)
        : base("MediaFile", mediaFileId)
    {
    }

    public MediaFileNotFoundException(string fileName)
        : base("MediaFile", fileName)
    {
    }
}
