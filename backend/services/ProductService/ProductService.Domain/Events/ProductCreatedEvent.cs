using Shared.Common.Domain;

namespace ProductService.Domain.Events;

public sealed record ProductCreatedEvent(int ProductId, string ProductName, string Sku) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
