using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Products;

public record SearchProductsQuery(string SearchTerm) : IRequest<Result<List<ProductDto>>>;
