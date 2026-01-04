namespace ProductService.Contracts.DTOs;

public record CreateProductDto(
    string Name,
    string Description,
    string Sku,
    decimal Price,
    string Currency,
    int CategoryId
);
