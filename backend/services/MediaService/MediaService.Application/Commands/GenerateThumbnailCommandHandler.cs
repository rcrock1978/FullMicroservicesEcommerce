using MediatR;
using MediaService.Contracts.DTOs;
using MediaService.Domain.Enums;
using MediaService.Domain.Exceptions;
using MediaService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace MediaService.Application.Commands;

public class GenerateThumbnailCommandHandler : IRequestHandler<GenerateThumbnailCommand, Result<ImageMetadataDto>>
{
    private readonly IMediaFileRepository _mediaFileRepository;
    private readonly IImageMetadataRepository _imageMetadataRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageProcessingService _imageProcessingService;

    public GenerateThumbnailCommandHandler(
        IMediaFileRepository mediaFileRepository,
        IImageMetadataRepository imageMetadataRepository,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        IImageProcessingService imageProcessingService)
    {
        _mediaFileRepository = mediaFileRepository;
        _imageMetadataRepository = imageMetadataRepository;
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _imageProcessingService = imageProcessingService;
    }

    public async Task<Result<ImageMetadataDto>> Handle(GenerateThumbnailCommand request, CancellationToken cancellationToken)
    {
        var mediaFile = await _mediaFileRepository.GetByIdWithMetadataAsync(request.MediaFileId, cancellationToken);
        
        if (mediaFile == null)
            return Result<ImageMetadataDto>.Failure("Media file not found");

        if (mediaFile.MediaType != MediaType.Image)
            return Result<ImageMetadataDto>.Failure("File is not an image");

        if (mediaFile.ImageMetadata == null)
            return Result<ImageMetadataDto>.Failure("Image metadata not found");

        // Download original image
        var downloadResult = await _fileStorageService.DownloadFileAsync(
            mediaFile.StoragePath,
            cancellationToken);

        if (!downloadResult.IsSuccess)
            return Result<ImageMetadataDto>.Failure($"Failed to download file: {downloadResult.Error}");

        // Generate thumbnail
        var thumbnailResult = await _imageProcessingService.GenerateThumbnailAsync(
            downloadResult.Value,
            request.Width,
            request.Height,
            request.MaintainAspectRatio,
            cancellationToken);

        if (!thumbnailResult.IsSuccess)
            return Result<ImageMetadataDto>.Failure($"Failed to generate thumbnail: {thumbnailResult.Error}");

        // Upload thumbnail
        var thumbnailFileName = $"thumb_{mediaFile.FileName}";
        var uploadResult = await _fileStorageService.UploadFileAsync(
            thumbnailResult.Value.Stream,
            thumbnailFileName,
            mediaFile.ContentType,
            cancellationToken);

        if (!uploadResult.IsSuccess)
            return Result<ImageMetadataDto>.Failure($"Failed to upload thumbnail: {uploadResult.Error}");

        // Update metadata
        mediaFile.ImageMetadata.SetThumbnail(
            uploadResult.Value.StoragePath,
            uploadResult.Value.PublicUrl,
            thumbnailResult.Value.Width,
            thumbnailResult.Value.Height
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new ImageMetadataDto(
            mediaFile.ImageMetadata.Id,
            mediaFile.ImageMetadata.MediaFileId,
            mediaFile.ImageMetadata.Width,
            mediaFile.ImageMetadata.Height,
            mediaFile.ImageMetadata.Format,
            mediaFile.ImageMetadata.HasThumbnail,
            mediaFile.ImageMetadata.ThumbnailPath,
            mediaFile.ImageMetadata.ThumbnailUrl,
            mediaFile.ImageMetadata.ThumbnailWidth,
            mediaFile.ImageMetadata.ThumbnailHeight
        );

        return Result<ImageMetadataDto>.Success(dto);
    }
}
