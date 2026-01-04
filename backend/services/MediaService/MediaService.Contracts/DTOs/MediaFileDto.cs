using MediaService.Domain.Enums;

namespace MediaService.Contracts.DTOs;

public record MediaFileDto(
    int Id,
    string FileName,
    string OriginalFileName,
    string ContentType,
    long FileSizeBytes,
    string StoragePath,
    string? PublicUrl,
    MediaType MediaType,
    StorageProvider StorageProvider,
    int? EntityId,
    string? EntityType,
    int UploadedByUserId,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    ImageMetadataDto? ImageMetadata
);
