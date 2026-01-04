namespace ProductService.Contracts.DTOs;

public record UpdateProductDto(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string Status
);
