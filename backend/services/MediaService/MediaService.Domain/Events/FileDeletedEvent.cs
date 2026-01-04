using Shared.Common.Domain;

namespace MediaService.Domain.Events;

public record FileDeletedEvent : IDomainEvent
{
    public int MediaFileId { get; init; }
    public string FileName { get; init; }
    public string StoragePath { get; init; }
    public int DeletedByUserId { get; init; }
    public DateTime OccurredOn { get; init; }

    public FileDeletedEvent(
        int mediaFileId,
        string fileName,
        string storagePath,
        int deletedByUserId)
    {
        MediaFileId = mediaFileId;
        FileName = fileName;
        StoragePath = storagePath;
        DeletedByUserId = deletedByUserId;
        OccurredOn = DateTime.UtcNow;
    }
}
