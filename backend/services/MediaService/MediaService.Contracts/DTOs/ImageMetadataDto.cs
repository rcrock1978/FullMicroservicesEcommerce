namespace MediaService.Contracts.DTOs;

public record ImageMetadataDto(
    int Id,
    int MediaFileId,
    int Width,
    int Height,
    string Format,
    bool HasThumbnail,
    string? ThumbnailPath,
    string? ThumbnailUrl,
    int? ThumbnailWidth,
    int? ThumbnailHeight
);
