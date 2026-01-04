using PaymentService.Domain.Enums;

namespace PaymentService.Contracts.Dtos;

public class CreatePaymentDto
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentMethodRequestDto PaymentMethod { get; set; } = null!;
}

public class PaymentMethodRequestDto
{
    public PaymentMethodType Type { get; set; }
    public string? CardBrand { get; set; }
    public string? Last4Digits { get; set; }
    public string? StripePaymentMethodId { get; set; }
}
