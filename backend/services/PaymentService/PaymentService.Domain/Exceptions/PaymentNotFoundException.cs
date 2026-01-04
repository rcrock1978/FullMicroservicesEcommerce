using Shared.Common.Domain.Exceptions;

namespace PaymentService.Domain.Exceptions;

public class PaymentNotFoundException : NotFoundException
{
    public PaymentNotFoundException(int paymentId)
        : base("Payment", paymentId, $"Payment with ID '{paymentId}' was not found.")
    {
    }

    public PaymentNotFoundException(string paymentIntentId)
        : base("Payment", paymentIntentId, $"Payment with PaymentIntentId '{paymentIntentId}' was not found.")
    {
    }
}
