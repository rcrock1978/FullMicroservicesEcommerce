using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(n => n.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(n => n.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(n => n.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Message)
            .IsRequired();

        builder.Property(n => n.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(n => n.FailureReason)
            .HasMaxLength(500);

        builder.Property(n => n.TemplateId)
            .HasMaxLength(100);

        builder.Property(n => n.ReferenceType)
            .HasMaxLength(50);

        // Indexes
        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => n.Status);
        builder.HasIndex(n => n.Type);
        builder.HasIndex(n => n.CreatedAt);
        builder.HasIndex(n => new { n.ReferenceType, n.ReferenceId });
    }
}
