namespace PaymentService.Application.Services;

public interface IStripeService
{
    Task<string> CreatePaymentIntentAsync(
        decimal amount,
        string currency,
        string orderNumber,
        CancellationToken cancellationToken = default);

    Task<(string ChargeId, string? CardLastFour, string? CardBrand)> ConfirmPaymentAsync(
        string paymentIntentId,
        CancellationToken cancellationToken = default);

    Task RefundPaymentAsync(
        string chargeId,
        decimal amount,
        CancellationToken cancellationToken = default);

    Task<bool> ValidateWebhookSignatureAsync(string payload, string signature);
}
