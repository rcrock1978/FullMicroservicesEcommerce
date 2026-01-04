using OrderService.Domain.ValueObjects;
using Shared.Common.Domain;

namespace OrderService.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public string? ImageUrl { get; private set; }

    private OrderItem() 
    { 
        ProductName = string.Empty;
        UnitPrice = new Money(0);
    } // EF Core

    public OrderItem(int productId, string productName, Money unitPrice, int quantity, string? imageUrl = null)
    {
        if (productId <= 0)
            throw new ArgumentException("Product ID must be greater than 0", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        ImageUrl = imageUrl;
    }

    public Money GetTotalPrice()
    {
        return new Money(UnitPrice.Amount * Quantity, UnitPrice.Currency);
    }
}
