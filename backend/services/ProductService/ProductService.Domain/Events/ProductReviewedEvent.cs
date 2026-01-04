using Shared.Common.Domain;

namespace ProductService.Domain.Events;

public sealed record ProductReviewedEvent(int ProductId, int ReviewId, int RatingValue) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
