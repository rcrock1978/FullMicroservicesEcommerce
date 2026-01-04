using Shared.Common.Domain;

namespace ProductService.Domain.Events;

public sealed record ProductUpdatedEvent(int ProductId, string ProductName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
