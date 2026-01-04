using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.ExpiresAt);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasMany<CartItem>()
            .WithOne()
            .HasForeignKey("CartId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.UserId);
        builder.HasIndex(c => c.ExpiresAt);
    }
}
