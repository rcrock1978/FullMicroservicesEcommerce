namespace ProductService.Contracts.DTOs;

public record ProductImageDto(
    int Id,
    string Url,
    string AltText,
    bool IsPrimary,
    int DisplayOrder
);
