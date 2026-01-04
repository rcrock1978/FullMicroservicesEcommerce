using PaymentService.Domain.Enums;

namespace PaymentService.Contracts.Dtos;

public record PaymentDto(
    int Id,
    int UserId,
    int OrderId,
    string OrderNumber,
    decimal Amount,
    string Currency,
    PaymentStatus Status,
    PaymentType PaymentType,
    string? StripePaymentIntentId,
    string? StripeChargeId,
    string? CardLastFourDigits,
    string? CardBrand,
    string? FailureReason,
    DateTime? ProcessedAt,
    DateTime? RefundedAt,
    decimal? RefundedAmount,
    DateTime CreatedAt
);
