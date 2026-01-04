namespace ProductService.Contracts.DTOs;

public record InventoryDto(
    int Id,
    int ProductId,
    int QuantityOnHand,
    int ReservedQuantity,
    int AvailableQuantity,
    int ReorderLevel,
    int ReorderQuantity,
    string Status
);
