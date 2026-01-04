using FluentValidation;
using PaymentService.Application.Commands;

namespace PaymentService.Application.Validators;

public class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .GreaterThan(0).WithMessage("Payment ID must be greater than zero");

        RuleFor(x => x.RefundDto.Amount)
            .GreaterThan(0).WithMessage("Refund amount must be greater than zero");

        RuleFor(x => x.RefundDto.Reason)
            .MaximumLength(500).WithMessage("Refund reason cannot exceed 500 characters");
    }
}
