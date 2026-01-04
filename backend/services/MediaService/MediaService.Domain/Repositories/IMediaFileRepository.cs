using MediaService.Domain.Entities;
using MediaService.Domain.Enums;
using Shared.Common.Domain;

namespace MediaService.Domain.Repositories;

public interface IMediaFileRepository : IRepository<MediaFile>
{
    Task<MediaFile?> GetByIdWithMetadataAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MediaFile>> GetByEntityAsync(int entityId, string entityType, CancellationToken cancellationToken = default);
    Task<IEnumerable<MediaFile>> GetByUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MediaFile>> GetByMediaTypeAsync(MediaType mediaType, CancellationToken cancellationToken = default);
    Task<bool> ExistsByFileNameAsync(string fileName, CancellationToken cancellationToken = default);
    Task<long> GetTotalStorageSizeByUserAsync(int userId, CancellationToken cancellationToken = default);
}
