using Shared.Common.Domain.Exceptions;

namespace CartService.Domain.Exceptions;

public class CartNotFoundException : NotFoundException
{
    public CartNotFoundException(int userId)
        : base("Cart", userId)
    {
    }
}
