using MediatR;
using MediaService.Contracts.DTOs;
using MediaService.Domain.Entities;
using MediaService.Domain.Enums;
using MediaService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace MediaService.Application.Commands;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Result<MediaFileDto>>
{
    private readonly IMediaFileRepository _mediaFileRepository;
    private readonly IImageMetadataRepository _imageMetadataRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly IImageProcessingService _imageProcessingService;

    public UploadFileCommandHandler(
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

    public async Task<Result<MediaFileDto>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Determine media type if not provided
            var mediaType = request.MediaType ?? DetermineMediaType(request.ContentType);

            // Generate unique filename
            var uniqueFileName = GenerateUniqueFileName(request.FileName);

            // Upload file to storage
            var uploadResult = await _fileStorageService.UploadFileAsync(
                request.FileStream,
                uniqueFileName,
                request.ContentType,
                cancellationToken);

            if (!uploadResult.IsSuccess)
                return Result<MediaFileDto>.Failure(uploadResult.Error);

            // Create MediaFile entity
            var mediaFile = new MediaFile(
                uniqueFileName,
                request.FileName,
                request.ContentType,
                request.FileSize,
                uploadResult.Value.StoragePath,
                mediaType,
                request.StorageProvider,
                request.UserId,
                uploadResult.Value.PublicUrl,
                request.EntityId,
                request.EntityType
            );

            await _mediaFileRepository.AddAsync(mediaFile, cancellationToken);

            // Process image metadata if it's an image
            if (mediaType == MediaType.Image)
            {
                var metadataResult = await _imageProcessingService.ExtractMetadataAsync(
                    request.FileStream,
                    cancellationToken);

                if (metadataResult.IsSuccess)
                {
                    var imageMetadata = new ImageMetadata(
                        mediaFile.Id,
                        metadataResult.Value.Width,
                        metadataResult.Value.Height,
                        metadataResult.Value.Format
                    );

                    await _imageMetadataRepository.AddAsync(imageMetadata, cancellationToken);
                    mediaFile.SetImageMetadata(imageMetadata);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = MapToDto(mediaFile);
            return Result<MediaFileDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<MediaFileDto>.Failure($"Failed to upload file: {ex.Message}");
        }
    }

    private MediaType DetermineMediaType(string contentType)
    {
        return contentType.ToLower() switch
        {
            var ct when ct.StartsWith("image/") => MediaType.Image,
            var ct when ct.StartsWith("video/") => MediaType.Video,
            var ct when ct.StartsWith("audio/") => MediaType.Audio,
            var ct when ct.Contains("pdf") || ct.Contains("document") => MediaType.Document,
            _ => MediaType.Other
        };
    }

    private string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var uniqueId = Guid.NewGuid().ToString("N")[..8];
        return $"{fileNameWithoutExtension}_{uniqueId}{extension}";
    }

    private MediaFileDto MapToDto(MediaFile mediaFile)
    {
        ImageMetadataDto? imageMetadataDto = null;
        if (mediaFile.ImageMetadata != null)
        {
            imageMetadataDto = new ImageMetadataDto(
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
        }

        return new MediaFileDto(
            mediaFile.Id,
            mediaFile.FileName,
            mediaFile.OriginalFileName,
            mediaFile.ContentType,
            mediaFile.FileSizeBytes,
            mediaFile.StoragePath,
            mediaFile.PublicUrl,
            mediaFile.MediaType,
            mediaFile.StorageProvider,
            mediaFile.EntityId,
            mediaFile.EntityType,
            mediaFile.UploadedByUserId,
            mediaFile.IsDeleted,
            mediaFile.CreatedAt,
            mediaFile.UpdatedAt,
            imageMetadataDto
        );
    }
}
