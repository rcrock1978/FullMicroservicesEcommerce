namespace ProductService.Contracts.DTOs;

public record CreateCategoryDto(
    string Name,
    string Description,
    string Slug,
    int? ParentCategoryId,
    int DisplayOrder
);
