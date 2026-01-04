using Microsoft.Extensions.Configuration;
using PaymentService.Application.Services;
using PaymentService.Domain.Exceptions;
using Stripe;

namespace PaymentService.Infrastructure.Services;

public class StripeService : IStripeService
{
    private readonly string _apiKey;

    public StripeService(IConfiguration configuration)
    {
        _apiKey = configuration["Stripe:SecretKey"] 
            ?? throw new ArgumentException("Stripe SecretKey not configured");
        
        StripeConfiguration.ApiKey = _apiKey;
    }

    public async Task<string> CreatePaymentIntentAsync(
        decimal amount,
        string currency,
        string orderNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe expects amount in cents
                Currency = currency.ToLower(),
                Metadata = new Dictionary<string, string>
                {
                    { "order_number", orderNumber }
                },
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options, cancellationToken: cancellationToken);

            return paymentIntent.Id;
        }
        catch (StripeException ex)
        {
            throw new PaymentProcessingException($"Stripe error: {ex.Message}", ex);
        }
    }

    public async Task<(string ChargeId, string? CardLastFour, string? CardBrand)> ConfirmPaymentAsync(
        string paymentIntentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId, cancellationToken: cancellationToken);

            if (paymentIntent.Status != "succeeded")
            {
                throw new PaymentProcessingException($"Payment intent status is {paymentIntent.Status}, not succeeded");
            }

            var chargeId = paymentIntent.LatestChargeId;
            string? cardLastFour = null;
            string? cardBrand = null;

            if (paymentIntent.PaymentMethodId != null)
            {
                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = await paymentMethodService.GetAsync(
                    paymentIntent.PaymentMethodId,
                    cancellationToken: cancellationToken);

                cardLastFour = paymentMethod.Card?.Last4;
                cardBrand = paymentMethod.Card?.Brand;
            }

            return (chargeId ?? string.Empty, cardLastFour, cardBrand);
        }
        catch (StripeException ex)
        {
            throw new PaymentProcessingException($"Stripe error: {ex.Message}", ex);
        }
    }

    public async Task RefundPaymentAsync(
        string chargeId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                Charge = chargeId,
                Amount = (long)(amount * 100) // Stripe expects amount in cents
            };

            var service = new RefundService();
            await service.CreateAsync(options, cancellationToken: cancellationToken);
        }
        catch (StripeException ex)
        {
            throw new PaymentProcessingException($"Stripe error: {ex.Message}", ex);
        }
    }

    public Task<bool> ValidateWebhookSignatureAsync(string payload, string signature)
    {
        try
        {
            var webhookSecret = "whsec_test"; // This should come from configuration
            var stripeEvent = EventUtility.ConstructEvent(payload, signature, webhookSecret);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
