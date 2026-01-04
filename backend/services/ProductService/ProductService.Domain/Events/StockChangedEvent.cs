using Shared.Common.Domain;

namespace ProductService.Domain.Events;

public sealed record StockChangedEvent(int ProductId, int QuantityOnHand, int AvailableQuantity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
