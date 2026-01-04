using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Categories;

public record GetCategoriesQuery : IRequest<Result<List<CategoryDto>>>;
