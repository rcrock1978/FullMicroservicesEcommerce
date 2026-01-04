namespace ProductService.Contracts.DTOs;

public record ProductDetailDto(
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
    List<ReviewDto> Reviews,
    InventoryDto? Inventory,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
