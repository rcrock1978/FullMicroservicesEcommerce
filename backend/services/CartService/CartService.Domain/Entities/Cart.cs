using CartService.Domain.ValueObjects;
using Shared.Common.Domain;

namespace CartService.Domain.Entities;

public class Cart : AggregateRoot
{
    private readonly List<CartItem> _items = new();

    public int UserId { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public DateTime? ExpiresAt { get; private set; }

    private Cart() { } // EF Core

    public Cart(int userId, int expirationMinutes = 30)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0", nameof(userId));

        UserId = userId;
        ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
    }

    public void AddItem(int productId, string productName, decimal unitPrice, int quantity, string? imageUrl = null)
    {
        if (productId <= 0)
            throw new ArgumentException("Product ID must be greater than 0", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var cartItem = new CartItem(productId, productName, new Money(unitPrice), quantity, imageUrl);
            _items.Add(cartItem);
        }

        RefreshExpiration();
    }

    public void UpdateItemQuantity(int productId, int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        
        if (item == null)
            throw new InvalidOperationException($"Product {productId} not found in cart");

        if (quantity == 0)
        {
            _items.Remove(item);
        }
        else
        {
            item.UpdateQuantity(quantity);
        }

        RefreshExpiration();
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        
        if (item != null)
        {
            _items.Remove(item);
            RefreshExpiration();
        }
    }

    public void Clear()
    {
        _items.Clear();
        RefreshExpiration();
    }

    public Money GetSubtotal()
    {
        if (!_items.Any())
            return new Money(0, "USD");

        var total = _items.Sum(i => i.GetTotalPrice().Amount);
        return new Money(total, _items.First().UnitPrice.Currency);
    }

    public int GetTotalItems()
    {
        return _items.Sum(i => i.Quantity);
    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    }

    public void RefreshExpiration(int expirationMinutes = 30)
    {
        ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
    }

    public bool HasItem(int productId)
    {
        return _items.Any(i => i.ProductId == productId);
    }
}
