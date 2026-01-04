namespace ProductService.Contracts.DTOs;

public record CategoryDto(
    int Id,
    string Name,
    string Description,
    string Slug,
    int? ParentCategoryId,
    string? ParentCategoryName,
    int DisplayOrder,
    bool IsActive,
    DateTime CreatedAt
);
