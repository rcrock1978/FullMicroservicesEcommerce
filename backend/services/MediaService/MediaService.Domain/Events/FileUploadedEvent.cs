using Shared.Common.Domain;

namespace MediaService.Domain.Events;

public record FileUploadedEvent : IDomainEvent
{
    public int MediaFileId { get; init; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
    public long FileSizeBytes { get; init; }
    public string MediaType { get; init; }
    public int UploadedByUserId { get; init; }
    public int? EntityId { get; init; }
    public string? EntityType { get; init; }
    public DateTime OccurredOn { get; init; }

    public FileUploadedEvent(
        int mediaFileId,
        string fileName,
        string contentType,
        long fileSizeBytes,
        string mediaType,
        int uploadedByUserId,
        int? entityId = null,
        string? entityType = null)
    {
        MediaFileId = mediaFileId;
        FileName = fileName;
        ContentType = contentType;
        FileSizeBytes = fileSizeBytes;
        MediaType = mediaType;
        UploadedByUserId = uploadedByUserId;
        EntityId = entityId;
        EntityType = entityType;
        OccurredOn = DateTime.UtcNow;
    }
}
