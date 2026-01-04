using CartService.Domain.Entities;
using CartService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.ProductId)
            .IsRequired();

        builder.Property(ci => ci.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(ci => ci.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("UnitPrice")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.ImageUrl)
            .HasMaxLength(500);

        builder.Property(ci => ci.CreatedAt)
            .IsRequired();

        builder.Property(ci => ci.UpdatedAt)
            .IsRequired();

        builder.HasIndex(ci => ci.ProductId);
    }
}
