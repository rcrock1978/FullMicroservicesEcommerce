using Shared.Common.Domain.Exceptions;

namespace CartService.Domain.Exceptions;

public class CartItemNotFoundException : NotFoundException
{
    public CartItemNotFoundException(int productId)
        : base("CartItem", productId)
    {
    }
}
