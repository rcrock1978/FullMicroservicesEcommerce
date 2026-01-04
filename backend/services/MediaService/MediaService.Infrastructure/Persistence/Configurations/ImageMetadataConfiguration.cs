using MediaService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaService.Infrastructure.Persistence.Configurations;

public class ImageMetadataConfiguration : IEntityTypeConfiguration<ImageMetadata>
{
    public void Configure(EntityTypeBuilder<ImageMetadata> builder)
    {
        builder.ToTable("ImageMetadata");

        builder.HasKey(im => im.Id);

        builder.Property(im => im.MediaFileId)
            .IsRequired();

        builder.Property(im => im.Width)
            .IsRequired();

        builder.Property(im => im.Height)
            .IsRequired();

        builder.Property(im => im.Format)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(im => im.HasThumbnail)
            .IsRequired();

        builder.Property(im => im.ThumbnailPath)
            .HasMaxLength(500);

        builder.Property(im => im.ThumbnailUrl)
            .HasMaxLength(1000);

        builder.Property(im => im.ThumbnailWidth);

        builder.Property(im => im.ThumbnailHeight);

        builder.HasIndex(im => im.MediaFileId)
            .IsUnique();
    }
}
