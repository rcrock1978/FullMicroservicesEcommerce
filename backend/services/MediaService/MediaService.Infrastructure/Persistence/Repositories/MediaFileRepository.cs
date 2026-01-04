using MediaService.Domain.Entities;
using MediaService.Domain.Enums;
using MediaService.Domain.Repositories;
using MediaService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Infrastructure.Persistence;

namespace MediaService.Infrastructure.Persistence.Repositories;

public class MediaFileRepository : Repository<MediaFile>, IMediaFileRepository
{
    public MediaFileRepository(MediaDbContext context) : base(context)
    {
    }

    public async Task<MediaFile?> GetByIdWithMetadataAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MediaFile>()
            .Include(mf => mf.ImageMetadata)
            .FirstOrDefaultAsync(mf => mf.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MediaFile>> GetByEntityAsync(int entityId, string entityType, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MediaFile>()
            .Include(mf => mf.ImageMetadata)
            .Where(mf => mf.EntityId == entityId && mf.EntityType == entityType && !mf.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MediaFile>> GetByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MediaFile>()
            .Include(mf => mf.ImageMetadata)
            .Where(mf => mf.UploadedByUserId == userId && !mf.IsDeleted)
            .OrderByDescending(mf => mf.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MediaFile>> GetByMediaTypeAsync(MediaType mediaType, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MediaFile>()
            .Where(mf => mf.MediaType == mediaType && !mf.IsDeleted)
            .OrderByDescending(mf => mf.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByFileNameAsync(string fileName, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MediaFile>()
            .AnyAsync(mf => mf.FileName == fileName, cancellationToken);
    }

    public async Task<long> GetTotalStorageSizeByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MediaFile>()
            .Where(mf => mf.UploadedByUserId == userId && !mf.IsDeleted)
            .SumAsync(mf => mf.FileSizeBytes, cancellationToken);
    }
}
