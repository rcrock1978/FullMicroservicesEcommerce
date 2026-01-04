using Shared.Common.Domain;
using Shared.Common.Domain.Exceptions;

namespace PaymentService.Domain.Exceptions;

public class PaymentNotFoundException : NotFoundException
{
    public PaymentNotFoundException(int paymentId)
        : base("Payment", paymentId)
    {
    }

    public PaymentNotFoundException(string stripePaymentIntentId)
        : base("Payment", stripePaymentIntentId, $"Payment with Stripe payment intent ID '{stripePaymentIntentId}' was not found")
    {
    }
}
