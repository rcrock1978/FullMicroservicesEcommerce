using MediatR;
using MediaService.Contracts.DTOs;
using MediaService.Domain.Repositories;
using Shared.Common.Application;

namespace MediaService.Application.Queries;

public class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, Result<MediaFileDto>>
{
    private readonly IMediaFileRepository _mediaFileRepository;

    public GetFileByIdQueryHandler(IMediaFileRepository mediaFileRepository)
    {
        _mediaFileRepository = mediaFileRepository;
    }

    public async Task<Result<MediaFileDto>> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var mediaFile = await _mediaFileRepository.GetByIdWithMetadataAsync(request.MediaFileId, cancellationToken);
        
        if (mediaFile == null)
            return Result<MediaFileDto>.Failure("Media file not found");

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

        var dto = new MediaFileDto(
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

        return Result<MediaFileDto>.Success(dto);
    }
}
