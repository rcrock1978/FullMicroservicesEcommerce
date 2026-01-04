using Shared.Common.Domain.Exceptions;

namespace OrderService.Domain.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(int orderId)
        : base("Order", orderId)
    {
    }

    public OrderNotFoundException(string orderNumber)
        : base($"Order with number '{orderNumber}' was not found")
    {
    }
}
