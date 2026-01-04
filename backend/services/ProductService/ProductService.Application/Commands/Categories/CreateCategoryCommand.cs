using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Commands.Categories;

public record CreateCategoryCommand(
    string Name,
    string Description,
    string Slug,
    int? ParentCategoryId,
    int DisplayOrder
) : IRequest<Result<CategoryDto>>;
