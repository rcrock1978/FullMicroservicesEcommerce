using MediatR;
using MediaService.Contracts.DTOs;
using MediaService.Domain.Repositories;
using Shared.Common.Application;

namespace MediaService.Application.Queries;

public class GetFilesByEntityQueryHandler : IRequestHandler<GetFilesByEntityQuery, Result<IEnumerable<MediaFileDto>>>
{
    private readonly IMediaFileRepository _mediaFileRepository;

    public GetFilesByEntityQueryHandler(IMediaFileRepository mediaFileRepository)
    {
        _mediaFileRepository = mediaFileRepository;
    }

    public async Task<Result<IEnumerable<MediaFileDto>>> Handle(GetFilesByEntityQuery request, CancellationToken cancellationToken)
    {
        var mediaFiles = await _mediaFileRepository.GetByEntityAsync(
            request.EntityId,
            request.EntityType,
            cancellationToken);

        var dtos = mediaFiles.Select(mf =>
        {
            ImageMetadataDto? imageMetadataDto = null;
            if (mf.ImageMetadata != null)
            {
                imageMetadataDto = new ImageMetadataDto(
                    mf.ImageMetadata.Id,
                    mf.ImageMetadata.MediaFileId,
                    mf.ImageMetadata.Width,
                    mf.ImageMetadata.Height,
                    mf.ImageMetadata.Format,
                    mf.ImageMetadata.HasThumbnail,
                    mf.ImageMetadata.ThumbnailPath,
                    mf.ImageMetadata.ThumbnailUrl,
                    mf.ImageMetadata.ThumbnailWidth,
                    mf.ImageMetadata.ThumbnailHeight
                );
            }

            return new MediaFileDto(
                mf.Id,
                mf.FileName,
                mf.OriginalFileName,
                mf.ContentType,
                mf.FileSizeBytes,
                mf.StoragePath,
                mf.PublicUrl,
                mf.MediaType,
                mf.StorageProvider,
                mf.EntityId,
                mf.EntityType,
                mf.UploadedByUserId,
                mf.IsDeleted,
                mf.CreatedAt,
                mf.UpdatedAt,
                imageMetadataDto
            );
        });

        return Result<IEnumerable<MediaFileDto>>.Success(dtos);
    }
}
