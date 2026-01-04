using FluentValidation;
using PaymentService.Application.Commands;

namespace PaymentService.Application.Validators;

public class InitiatePaymentCommandValidator : AbstractValidator<InitiatePaymentCommand>
{
    public InitiatePaymentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than zero");

        RuleFor(x => x.CreatePaymentDto.OrderId)
            .GreaterThan(0).WithMessage("Order ID must be greater than zero");

        RuleFor(x => x.CreatePaymentDto.OrderNumber)
            .NotEmpty().WithMessage("Order number is required")
            .MaximumLength(50).WithMessage("Order number cannot exceed 50 characters");

        RuleFor(x => x.CreatePaymentDto.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

        RuleFor(x => x.CreatePaymentDto.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be 3 characters (ISO 4217)");
    }
}
