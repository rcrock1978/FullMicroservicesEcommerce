using MediaService.Domain.Enums;
using Shared.Common.Domain;

namespace MediaService.Domain.Entities;

public class MediaFile : BaseEntity
{
    public string FileName { get; private set; }
    public string OriginalFileName { get; private set; }
    public string ContentType { get; private set; }
    public long FileSizeBytes { get; private set; }
    public string StoragePath { get; private set; }
    public string? PublicUrl { get; private set; }
    public MediaType MediaType { get; private set; }
    public StorageProvider StorageProvider { get; private set; }
    public int? EntityId { get; private set; }
    public string? EntityType { get; private set; }
    public int UploadedByUserId { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    
    public ImageMetadata? ImageMetadata { get; private set; }

    private MediaFile() 
    {
        FileName = string.Empty;
        OriginalFileName = string.Empty;
        ContentType = string.Empty;
        StoragePath = string.Empty;
    } // EF Core

    public MediaFile(
        string fileName,
        string originalFileName,
        string contentType,
        long fileSizeBytes,
        string storagePath,
        MediaType mediaType,
        StorageProvider storageProvider,
        int uploadedByUserId,
        string? publicUrl = null,
        int? entityId = null,
        string? entityType = null)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));

        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new ArgumentException("Original file name cannot be empty", nameof(originalFileName));

        if (fileSizeBytes <= 0)
            throw new ArgumentException("File size must be greater than 0", nameof(fileSizeBytes));

        FileName = fileName;
        OriginalFileName = originalFileName;
        ContentType = contentType;
        FileSizeBytes = fileSizeBytes;
        StoragePath = storagePath;
        PublicUrl = publicUrl;
        MediaType = mediaType;
        StorageProvider = storageProvider;
        EntityId = entityId;
        EntityType = entityType;
        UploadedByUserId = uploadedByUserId;
    }

    public void SetPublicUrl(string url)
    {
        PublicUrl = url;
    }

    public void AttachToEntity(int entityId, string entityType)
    {
        EntityId = entityId;
        EntityType = entityType;
    }

    public void SetImageMetadata(ImageMetadata metadata)
    {
        if (MediaType != MediaType.Image)
            throw new InvalidOperationException("Cannot set image metadata on non-image file");

        ImageMetadata = metadata;
    }

    public new void MarkAsDeleted()
    {
        base.MarkAsDeleted();
        DeletedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}
