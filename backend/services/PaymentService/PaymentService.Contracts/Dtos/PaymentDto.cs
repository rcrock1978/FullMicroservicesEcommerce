using PaymentService.Domain.Enums;

namespace PaymentService.Contracts.Dtos;

public class PaymentDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
    public PaymentMethodDto PaymentMethod { get; set; } = null!;
    public string? StripePaymentIntentId { get; set; }
    public string? StripeChargeId { get; set; }
    public string? FailureReason { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime? RefundedAt { get; set; }
    public decimal? RefundedAmount { get; set; }
    public string? RefundReason { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PaymentMethodDto
{
    public PaymentMethodType Type { get; set; }
    public string? CardBrand { get; set; }
    public string? Last4Digits { get; set; }
    public string? StripePaymentMethodId { get; set; }
}
