namespace ProductService.Contracts.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string Description,
    string Sku,
    decimal Price,
    string Currency,
    int CategoryId,
    string CategoryName,
    string Status,
    decimal AverageRating,
    int ReviewCount,
    List<ProductImageDto> Images,
    InventoryDto? Inventory,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
