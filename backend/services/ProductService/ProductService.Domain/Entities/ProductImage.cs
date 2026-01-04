using Shared.Common.Domain;

namespace ProductService.Domain.Entities;

public class ProductImage : BaseEntity
{
    public int ProductId { get; private set; }
    public Product Product { get; private set; }
    public string Url { get; private set; }
    public string AltText { get; private set; }
    public bool IsPrimary { get; private set; }
    public int DisplayOrder { get; private set; }

    private ProductImage() { } // EF Core

    public ProductImage(string url, string altText, bool isPrimary = false, int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL cannot be empty", nameof(url));

        Url = url;
        AltText = altText ?? string.Empty;
        IsPrimary = isPrimary;
        DisplayOrder = displayOrder;
    }

    public void Update(string url, string altText)
    {
        Url = url;
        AltText = altText ?? string.Empty;
    }

    public void SetAsPrimary() => IsPrimary = true;
    public void RemoveAsPrimary() => IsPrimary = false;
}
