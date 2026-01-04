using PaymentService.Domain.Enums;
using Shared.Common.Domain;
using Shared.Common.Domain.Exceptions;

namespace PaymentService.Domain.ValueObjects;

public class PaymentMethod
{
    public PaymentMethodType Type { get; private set; }
    public string? CardBrand { get; private set; }
    public string? Last4Digits { get; private set; }
    public string? StripePaymentMethodId { get; private set; }

    private PaymentMethod() { } // For EF Core

    public PaymentMethod(PaymentMethodType type, string? cardBrand = null, string? last4Digits = null, string? stripePaymentMethodId = null)
    {
        if (type == PaymentMethodType.CreditCard || type == PaymentMethodType.DebitCard)
        {
            if (string.IsNullOrWhiteSpace(last4Digits))
                throw new DomainValidationException("Last 4 digits are required for card payments");
        }

        Type = type;
        CardBrand = cardBrand;
        Last4Digits = last4Digits;
        StripePaymentMethodId = stripePaymentMethodId;
    }

    public override string ToString()
    {
        if (!string.IsNullOrWhiteSpace(CardBrand) && !string.IsNullOrWhiteSpace(Last4Digits))
            return $"{CardBrand} ending in {Last4Digits}";
        
        return Type.ToString();
    }
}
