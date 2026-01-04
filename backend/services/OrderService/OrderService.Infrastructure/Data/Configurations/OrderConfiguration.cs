using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserId)
            .IsRequired();

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.OwnsOne(o => o.SubtotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("SubtotalAmount")
                .HasPrecision(18, 2);
            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });

        builder.OwnsOne(o => o.ShippingCost, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("ShippingCost")
                .HasPrecision(18, 2);
            money.Property(m => m.Currency)
                .HasColumnName("ShippingCostCurrency")
                .HasMaxLength(3);
        });

        builder.OwnsOne(o => o.TaxAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TaxAmount")
                .HasPrecision(18, 2);
            money.Property(m => m.Currency)
                .HasColumnName("TaxCurrency")
                .HasMaxLength(3);
        });

        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2);
            money.Property(m => m.Currency)
                .HasColumnName("TotalCurrency")
                .HasMaxLength(3);
        });

        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("ShippingStreet")
                .HasMaxLength(200);
            address.Property(a => a.City)
                .HasColumnName("ShippingCity")
                .HasMaxLength(100);
            address.Property(a => a.State)
                .HasColumnName("ShippingState")
                .HasMaxLength(100);
            address.Property(a => a.PostalCode)
                .HasColumnName("ShippingPostalCode")
                .HasMaxLength(20);
            address.Property(a => a.Country)
                .HasColumnName("ShippingCountry")
                .HasMaxLength(100);
        });

        builder.OwnsOne(o => o.PaymentMethod, payment =>
        {
            payment.Property(p => p.Type)
                .HasColumnName("PaymentType")
                .HasConversion<string>();
            payment.Property(p => p.CardLastFourDigits)
                .HasColumnName("CardLastFourDigits")
                .HasMaxLength(4);
            payment.Property(p => p.PaymentIntentId)
                .HasColumnName("PaymentIntentId")
                .HasMaxLength(100);
        });

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        builder.Property(o => o.CancellationReason)
            .HasMaxLength(500);

        builder.HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.HasIndex(o => o.UserId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.CreatedAt);
    }
}
