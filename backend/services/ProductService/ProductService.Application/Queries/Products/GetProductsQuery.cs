using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Products;

public record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<Result<List<ProductDto>>>;
