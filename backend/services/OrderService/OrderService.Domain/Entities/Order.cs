using OrderService.Domain.Enums;
using OrderService.Domain.ValueObjects;
using Shared.Common.Domain;

namespace OrderService.Domain.Entities;

public class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();

    public int UserId { get; private set; }
    public string OrderNumber { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Address ShippingAddress { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public Money SubtotalAmount { get; private set; }
    public Money ShippingCost { get; private set; }
    public Money TaxAmount { get; private set; }
    public Money TotalAmount { get; private set; }
    public string? Notes { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }

    private Order() 
    { 
        OrderNumber = string.Empty;
        ShippingAddress = null!;
        PaymentMethod = null!;
        SubtotalAmount = new Money(0);
        ShippingCost = new Money(0);
        TaxAmount = new Money(0);
        TotalAmount = new Money(0);
    } // EF Core

    public Order(
        int userId,
        string orderNumber,
        Address shippingAddress,
        PaymentMethod paymentMethod,
        Money shippingCost,
        string? notes = null)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0", nameof(userId));

        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Order number is required", nameof(orderNumber));

        UserId = userId;
        OrderNumber = orderNumber;
        Status = OrderStatus.Pending;
        ShippingAddress = shippingAddress;
        PaymentMethod = paymentMethod;
        ShippingCost = shippingCost;
        SubtotalAmount = new Money(0);
        TaxAmount = new Money(0);
        TotalAmount = shippingCost;
        Notes = notes;
    }

    public void AddItem(int productId, string productName, decimal unitPrice, int quantity, string? imageUrl = null)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify order after it has been placed");

        if (productId <= 0)
            throw new ArgumentException("Product ID must be greater than 0", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        var orderItem = new OrderItem(productId, productName, new Money(unitPrice), quantity, imageUrl);
        _items.Add(orderItem);

        RecalculateTotals();
    }

    public void ConfirmOrder()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm order with status {Status}");

        if (!_items.Any())
            throw new InvalidOperationException("Cannot confirm order with no items");

        Status = OrderStatus.Confirmed;
    }

    public void StartProcessing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Cannot start processing order with status {Status}");

        Status = OrderStatus.Processing;
    }

    public void Ship(DateTime? shippedAt = null)
    {
        if (Status != OrderStatus.Processing)
            throw new InvalidOperationException($"Cannot ship order with status {Status}");

        Status = OrderStatus.Shipped;
        ShippedAt = shippedAt ?? DateTime.UtcNow;
    }

    public void Deliver(DateTime? deliveredAt = null)
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException($"Cannot deliver order with status {Status}");

        Status = OrderStatus.Delivered;
        DeliveredAt = deliveredAt ?? DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Delivered or OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel order with status {Status}");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required", nameof(reason));

        Status = OrderStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason;
    }

    private void RecalculateTotals()
    {
        SubtotalAmount = new Money(_items.Sum(i => i.GetTotalPrice().Amount));
        TaxAmount = new Money(SubtotalAmount.Amount * 0.1m); // 10% tax
        TotalAmount = SubtotalAmount + TaxAmount + ShippingCost;
    }

    public int GetTotalItems()
    {
        return _items.Sum(i => i.Quantity);
    }
}
