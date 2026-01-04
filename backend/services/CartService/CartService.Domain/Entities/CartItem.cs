using CartService.Domain.ValueObjects;
using Shared.Common.Domain;

namespace CartService.Domain.Entities;

public class CartItem : BaseEntity
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public string? ImageUrl { get; private set; }

    private CartItem() 
    {
        ProductName = string.Empty;
        UnitPrice = new Money(0, "USD");
    } // EF Core

    public CartItem(int productId, string productName, Money unitPrice, int quantity, string? imageUrl = null)
    {
        if (productId <= 0)
            throw new ArgumentException("Product ID must be greater than 0", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be empty", nameof(productName));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        ImageUrl = imageUrl;
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        Quantity = quantity;
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount < 0)
            throw new ArgumentException("Price cannot be negative");

        UnitPrice = newPrice;
    }

    public Money GetTotalPrice()
    {
        return new Money(UnitPrice.Amount * Quantity, UnitPrice.Currency);
    }
}
