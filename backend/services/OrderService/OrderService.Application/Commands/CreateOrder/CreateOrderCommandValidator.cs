using FluentValidation;

namespace OrderService.Application.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must have at least one item");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0");

            item.RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required");

            item.RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Unit price must be non-negative");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        });

        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .WithMessage("Shipping address is required");

        RuleFor(x => x.ShippingAddress.Street)
            .NotEmpty()
            .When(x => x.ShippingAddress != null)
            .WithMessage("Street is required");

        RuleFor(x => x.ShippingAddress.City)
            .NotEmpty()
            .When(x => x.ShippingAddress != null)
            .WithMessage("City is required");

        RuleFor(x => x.PaymentMethod)
            .NotNull()
            .WithMessage("Payment method is required");

        RuleFor(x => x.ShippingCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Shipping cost must be non-negative");
    }
}
