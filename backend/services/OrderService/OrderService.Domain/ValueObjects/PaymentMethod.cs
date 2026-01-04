using OrderService.Domain.Enums;

namespace OrderService.Domain.ValueObjects;

public record PaymentMethod
{
    public PaymentType Type { get; init; }
    public string? CardLastFourDigits { get; init; }
    public string? PaymentIntentId { get; init; }

    public PaymentMethod(PaymentType type, string? cardLastFourDigits = null, string? paymentIntentId = null)
    {
        Type = type;
        CardLastFourDigits = cardLastFourDigits;
        PaymentIntentId = paymentIntentId;
    }
}
