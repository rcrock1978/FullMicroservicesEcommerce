using ProductService.Domain.Enums;
using ProductService.Domain.Events;
using ProductService.Domain.ValueObjects;
using Shared.Common.Domain;

namespace ProductService.Domain.Entities;

public class Product : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sku Sku { get; private set; }
    public Money Price { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public ProductStatus Status { get; private set; }
    public Rating AverageRating { get; private set; }
    public int ReviewCount { get; private set; }
    public int TotalRating { get; private set; }
    
    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    
    private readonly List<Review> _reviews = new();
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
    
    public Inventory Inventory { get; private set; }

    private Product() { } // EF Core

    public Product(
        string name,
        string description,
        Sku sku,
        Money price,
        int categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        Sku = sku;
        Price = price;
        CategoryId = categoryId;
        Status = ProductStatus.Draft;
        AverageRating = new Rating(0);
        ReviewCount = 0;
        TotalRating = 0;

        AddDomainEvent(new ProductCreatedEvent(Id, name, sku.Value));
    }

    public void UpdateDetails(string name, string description, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        Price = price;

        AddDomainEvent(new ProductUpdatedEvent(Id, name));
    }

    public void ChangeStatus(ProductStatus status)
    {
        Status = status;
        AddDomainEvent(new ProductUpdatedEvent(Id, Name));
    }

    public void AddImage(string url, string altText, bool isPrimary = false)
    {
        var image = new ProductImage(url, altText, isPrimary);
        _images.Add(image);
    }

    public void RemoveImage(int imageId)
    {
        var image = _images.FirstOrDefault(i => i.Id == imageId);
        if (image != null)
        {
            _images.Remove(image);
        }
    }

    public void AddReview(Review review)
    {
        _reviews.Add(review);
        TotalRating += review.RatingValue;
        ReviewCount++;
        AverageRating = Rating.Create(TotalRating, ReviewCount);
        
        AddDomainEvent(new ProductReviewedEvent(Id, review.Id, review.RatingValue));
    }

    public void SetInventory(Inventory inventory)
    {
        Inventory = inventory;
    }
}
