using Shared.Common.Domain;

namespace CartService.Domain.Events;

public record CartItemRemovedEvent : IDomainEvent
{
    public int CartId { get; init; }
    public int UserId { get; init; }
    public int ProductId { get; init; }
    public DateTime OccurredOn { get; init; }

    public CartItemRemovedEvent(int cartId, int userId, int productId)
    {
        CartId = cartId;
        UserId = userId;
        ProductId = productId;
        OccurredOn = DateTime.UtcNow;
    }
}
