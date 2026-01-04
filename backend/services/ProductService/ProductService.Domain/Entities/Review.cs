using Shared.Common.Domain;

namespace ProductService.Domain.Entities;

public class Review : BaseEntity
{
    public int ProductId { get; private set; }
    public Product Product { get; private set; }
    public int UserId { get; private set; }
    public int RatingValue { get; private set; }
    public string Title { get; private set; }
    public string Comment { get; private set; }
    public bool IsVerifiedPurchase { get; private set; }
    public DateTime ReviewedAt { get; private set; }

    private Review() { } // EF Core

    public Review(int productId, int userId, int ratingValue, string title, string comment, bool isVerifiedPurchase = false)
    {
        if (ratingValue < 1 || ratingValue > 5)
            throw new ArgumentException("Rating must be between 1 and 5", nameof(ratingValue));

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Review comment cannot be empty", nameof(comment));

        ProductId = productId;
        UserId = userId;
        RatingValue = ratingValue;
        Title = title ?? string.Empty;
        Comment = comment;
        IsVerifiedPurchase = isVerifiedPurchase;
        ReviewedAt = DateTime.UtcNow;
    }

    public void Update(int ratingValue, string title, string comment)
    {
        if (ratingValue < 1 || ratingValue > 5)
            throw new ArgumentException("Rating must be between 1 and 5", nameof(ratingValue));

        RatingValue = ratingValue;
        Title = title ?? string.Empty;
        Comment = comment;
    }
}
