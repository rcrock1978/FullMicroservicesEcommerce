using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.PaymentType)
            .HasConversion<string>()
            .IsRequired();

        // Owned Money value object for Amount
        builder.OwnsOne(p => p.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Owned Money value object for RefundedAmount
        builder.OwnsOne(p => p.RefundedAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("RefundedAmountValue")
                .HasPrecision(18, 2);

            money.Property(m => m.Currency)
                .HasColumnName("RefundedAmountCurrency")
                .HasMaxLength(3);
        });

        builder.Property(p => p.StripePaymentIntentId)
            .HasMaxLength(100);

        builder.Property(p => p.StripeChargeId)
            .HasMaxLength(100);

        builder.Property(p => p.StripeCustomerId)
            .HasMaxLength(100);

        builder.Property(p => p.CardLastFourDigits)
            .HasMaxLength(4);

        builder.Property(p => p.CardBrand)
            .HasMaxLength(20);

        builder.Property(p => p.FailureReason)
            .HasMaxLength(500);

        builder.Property(p => p.FailureCode)
            .HasMaxLength(50);

        builder.Property(p => p.RefundReason)
            .HasMaxLength(500);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.OrderId).IsUnique();
        builder.HasIndex(p => p.OrderNumber);
        builder.HasIndex(p => p.StripePaymentIntentId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CreatedAt);
    }
}
