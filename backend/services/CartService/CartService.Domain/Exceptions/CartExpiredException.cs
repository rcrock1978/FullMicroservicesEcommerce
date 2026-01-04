using Shared.Common.Domain.Exceptions;

namespace CartService.Domain.Exceptions;

public class CartExpiredException : DomainValidationException
{
    public CartExpiredException()
        : base("Cart has expired")
    {
    }
}
