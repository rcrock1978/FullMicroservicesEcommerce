using MediaService.Domain.Entities;
using MediaService.Domain.Repositories;
using MediaService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Infrastructure.Persistence;

namespace MediaService.Infrastructure.Persistence.Repositories;

public class ImageMetadataRepository : Repository<ImageMetadata>, IImageMetadataRepository
{
    public ImageMetadataRepository(MediaDbContext context) : base(context)
    {
    }

    public async Task<ImageMetadata?> GetByMediaFileIdAsync(int mediaFileId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ImageMetadata>()
            .FirstOrDefaultAsync(im => im.MediaFileId == mediaFileId, cancellationToken);
    }

    public async Task<IEnumerable<ImageMetadata>> GetWithThumbnailsAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<ImageMetadata>()
            .Where(im => im.HasThumbnail)
            .Include(im => im.MediaFile)
            .ToListAsync(cancellationToken);
    }
}
