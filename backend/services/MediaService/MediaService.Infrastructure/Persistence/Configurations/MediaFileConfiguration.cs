using MediaService.Domain.Entities;
using MediaService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaService.Infrastructure.Persistence.Configurations;

public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.ToTable("MediaFiles");

        builder.HasKey(mf => mf.Id);

        builder.Property(mf => mf.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(mf => mf.OriginalFileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(mf => mf.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mf => mf.FileSizeBytes)
            .IsRequired();

        builder.Property(mf => mf.StoragePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(mf => mf.PublicUrl)
            .HasMaxLength(1000);

        builder.Property(mf => mf.MediaType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(mf => mf.StorageProvider)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(mf => mf.EntityId);

        builder.Property(mf => mf.EntityType)
            .HasMaxLength(100);

        builder.Property(mf => mf.UploadedByUserId)
            .IsRequired();

        builder.Property(mf => mf.IsDeleted)
            .IsRequired();

        builder.Property(mf => mf.DeletedAt);

        builder.Property(mf => mf.CreatedAt)
            .IsRequired();

        builder.Property(mf => mf.UpdatedAt);

        builder.HasOne(mf => mf.ImageMetadata)
            .WithOne(im => im.MediaFile)
            .HasForeignKey<ImageMetadata>(im => im.MediaFileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mf => mf.FileName);
        builder.HasIndex(mf => new { mf.EntityId, mf.EntityType });
        builder.HasIndex(mf => mf.UploadedByUserId);
        builder.HasIndex(mf => mf.MediaType);
        builder.HasIndex(mf => mf.IsDeleted);
    }
}
