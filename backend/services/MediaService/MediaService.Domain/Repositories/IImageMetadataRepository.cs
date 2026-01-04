using MediaService.Domain.Entities;
using Shared.Common.Domain;

namespace MediaService.Domain.Repositories;

public interface IImageMetadataRepository : IRepository<ImageMetadata>
{
    Task<ImageMetadata?> GetByMediaFileIdAsync(int mediaFileId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ImageMetadata>> GetWithThumbnailsAsync(CancellationToken cancellationToken = default);
}
