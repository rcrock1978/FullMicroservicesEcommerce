using MediaService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaService.Infrastructure.Persistence;

public class MediaDbContext : DbContext
{
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
    public DbSet<ImageMetadata> ImageMetadata => Set<ImageMetadata>();

    public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MediaDbContext).Assembly);
    }
}
