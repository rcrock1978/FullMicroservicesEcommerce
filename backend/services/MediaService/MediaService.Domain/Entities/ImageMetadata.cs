using Shared.Common.Domain;

namespace MediaService.Domain.Entities;

public class ImageMetadata : BaseEntity
{
    public int MediaFileId { get; private set; }
    public MediaFile MediaFile { get; private set; } = null!;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public string Format { get; private set; }
    public bool HasThumbnail { get; private set; }
    public string? ThumbnailPath { get; private set; }
    public string? ThumbnailUrl { get; private set; }
    public int? ThumbnailWidth { get; private set; }
    public int? ThumbnailHeight { get; private set; }

    private ImageMetadata() 
    {
        Format = string.Empty;
    } // EF Core

    public ImageMetadata(
        int mediaFileId,
        int width,
        int height,
        string format)
    {
        if (width <= 0)
            throw new ArgumentException("Width must be greater than 0", nameof(width));

        if (height <= 0)
            throw new ArgumentException("Height must be greater than 0", nameof(height));

        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentException("Format cannot be empty", nameof(format));

        MediaFileId = mediaFileId;
        Width = width;
        Height = height;
        Format = format;
        HasThumbnail = false;
    }

    public void SetThumbnail(string thumbnailPath, string? thumbnailUrl, int width, int height)
    {
        ThumbnailPath = thumbnailPath;
        ThumbnailUrl = thumbnailUrl;
        ThumbnailWidth = width;
        ThumbnailHeight = height;
        HasThumbnail = true;
    }

    public void RemoveThumbnail()
    {
        ThumbnailPath = null;
        ThumbnailUrl = null;
        ThumbnailWidth = null;
        ThumbnailHeight = null;
        HasThumbnail = false;
    }
}
