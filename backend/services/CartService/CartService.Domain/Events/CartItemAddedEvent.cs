using Shared.Common.Domain;

namespace CartService.Domain.Events;

public record CartItemAddedEvent : IDomainEvent
{
    public int CartId { get; init; }
    public int UserId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public DateTime OccurredOn { get; init; }

    public CartItemAddedEvent(
        int cartId,
        int userId,
        int productId,
        string productName,
        int quantity,
        decimal unitPrice)
    {
        CartId = cartId;
        UserId = userId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        OccurredOn = DateTime.UtcNow;
    }
}
