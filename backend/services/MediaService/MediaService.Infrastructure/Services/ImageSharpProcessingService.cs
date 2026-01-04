using MediaService.Application;
using Shared.Common.Application;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace MediaService.Infrastructure.Services;

public class ImageSharpProcessingService : IImageProcessingService
{
    public async Task<Result<ImageMetadataResult>> ExtractMetadataAsync(
        Stream imageStream,
        CancellationToken cancellationToken = default)
    {
        try
        {
            imageStream.Position = 0;
            
            using var image = await Image.LoadAsync(imageStream, cancellationToken);
            
            var format = image.Metadata.DecodedImageFormat?.Name ?? "Unknown";
            
            var result = new ImageMetadataResult(
                image.Width,
                image.Height,
                format
            );

            return Result<ImageMetadataResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<ImageMetadataResult>.Failure($"Failed to extract image metadata: {ex.Message}");
        }
    }

    public async Task<Result<ThumbnailResult>> GenerateThumbnailAsync(
        Stream imageStream,
        int width,
        int height,
        bool maintainAspectRatio = true,
        CancellationToken cancellationToken = default)
    {
        try
        {
            imageStream.Position = 0;
            
            using var image = await Image.LoadAsync(imageStream, cancellationToken);
            
            var resizeOptions = new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = maintainAspectRatio ? ResizeMode.Max : ResizeMode.Stretch
            };

            image.Mutate(x => x.Resize(resizeOptions));

            var outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream, cancellationToken);
            outputStream.Position = 0;

            var result = new ThumbnailResult(
                outputStream,
                image.Width,
                image.Height
            );

            return Result<ThumbnailResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<ThumbnailResult>.Failure($"Failed to generate thumbnail: {ex.Message}");
        }
    }

    public async Task<Result<Stream>> ResizeImageAsync(
        Stream imageStream,
        int width,
        int height,
        CancellationToken cancellationToken = default)
    {
        try
        {
            imageStream.Position = 0;
            
            using var image = await Image.LoadAsync(imageStream, cancellationToken);
            
            image.Mutate(x => x.Resize(width, height));

            var outputStream = new MemoryStream();
            
            // Use appropriate encoder based on original format
            var encoder = image.Metadata.DecodedImageFormat?.Name?.ToLower() == "png"
                ? (IImageEncoder)new PngEncoder()
                : new JpegEncoder();
                
            await image.SaveAsync(outputStream, encoder, cancellationToken);
            outputStream.Position = 0;

            return Result<Stream>.Success(outputStream);
        }
        catch (Exception ex)
        {
            return Result<Stream>.Failure($"Failed to resize image: {ex.Message}");
        }
    }

    public async Task<Result<Stream>> OptimizeImageAsync(
        Stream imageStream,
        int quality = 85,
        CancellationToken cancellationToken = default)
    {
        try
        {
            imageStream.Position = 0;
            
            using var image = await Image.LoadAsync(imageStream, cancellationToken);
            
            var encoder = new JpegEncoder
            {
                Quality = quality
            };

            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder, cancellationToken);
            outputStream.Position = 0;

            return Result<Stream>.Success(outputStream);
        }
        catch (Exception ex)
        {
            return Result<Stream>.Failure($"Failed to optimize image: {ex.Message}");
        }
    }
}
