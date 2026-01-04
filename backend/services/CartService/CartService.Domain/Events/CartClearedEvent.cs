using Shared.Common.Domain;

namespace CartService.Domain.Events;

public record CartClearedEvent : IDomainEvent
{
    public int CartId { get; init; }
    public int UserId { get; init; }
    public DateTime OccurredOn { get; init; }

    public CartClearedEvent(int cartId, int userId)
    {
        CartId = cartId;
        UserId = userId;
        OccurredOn = DateTime.UtcNow;
    }
}
