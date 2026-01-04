using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Products;

public record GetProductsByCategoryQuery(int CategoryId) : IRequest<Result<List<ProductDto>>>;
