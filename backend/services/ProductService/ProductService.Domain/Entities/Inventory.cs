using ProductService.Domain.Enums;
using ProductService.Domain.Events;
using Shared.Common.Domain;

namespace ProductService.Domain.Entities;

public class Inventory : BaseEntity
{
    public int ProductId { get; private set; }
    public Product Product { get; private set; }
    public int QuantityOnHand { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int AvailableQuantity => QuantityOnHand - ReservedQuantity;
    public int ReorderLevel { get; private set; }
    public int ReorderQuantity { get; private set; }
    public StockStatus Status { get; private set; }

    private Inventory() { } // EF Core

    public Inventory(int productId, int quantityOnHand, int reorderLevel, int reorderQuantity)
    {
        ProductId = productId;
        QuantityOnHand = quantityOnHand;
        ReservedQuantity = 0;
        ReorderLevel = reorderLevel;
        ReorderQuantity = reorderQuantity;
        UpdateStatus();
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        QuantityOnHand += quantity;
        UpdateStatus();
        
        AddDomainEvent(new StockChangedEvent(ProductId, QuantityOnHand, AvailableQuantity));
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (QuantityOnHand < quantity)
            throw new InvalidOperationException("Insufficient stock");

        QuantityOnHand -= quantity;
        UpdateStatus();
        
        AddDomainEvent(new StockChangedEvent(ProductId, QuantityOnHand, AvailableQuantity));
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (AvailableQuantity < quantity)
            throw new InvalidOperationException("Insufficient available stock");

        ReservedQuantity += quantity;
        UpdateStatus();
    }

    public void ReleaseReservedStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (ReservedQuantity < quantity)
            throw new InvalidOperationException("Cannot release more than reserved");

        ReservedQuantity -= quantity;
        UpdateStatus();
    }

    public void SetReorderLevel(int reorderLevel, int reorderQuantity)
    {
        ReorderLevel = reorderLevel;
        ReorderQuantity = reorderQuantity;
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (AvailableQuantity == 0)
            Status = StockStatus.OutOfStock;
        else if (AvailableQuantity <= ReorderLevel)
            Status = StockStatus.LowStock;
        else
            Status = StockStatus.InStock;
    }
}
