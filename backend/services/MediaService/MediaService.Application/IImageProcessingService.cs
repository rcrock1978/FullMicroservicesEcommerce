using Shared.Common.Application;

namespace MediaService.Application;

public interface IImageProcessingService
{
    Task<Result<ImageMetadataResult>> ExtractMetadataAsync(
        Stream imageStream,
        CancellationToken cancellationToken = default);

    Task<Result<ThumbnailResult>> GenerateThumbnailAsync(
        Stream imageStream,
        int width,
        int height,
        bool maintainAspectRatio = true,
        CancellationToken cancellationToken = default);

    Task<Result<Stream>> ResizeImageAsync(
        Stream imageStream,
        int width,
        int height,
        CancellationToken cancellationToken = default);

    Task<Result<Stream>> OptimizeImageAsync(
        Stream imageStream,
        int quality = 85,
        CancellationToken cancellationToken = default);
}

public record ImageMetadataResult(
    int Width,
    int Height,
    string Format
);

public record ThumbnailResult(
    Stream Stream,
    int Width,
    int Height
);
