using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Queries.Products;

public record GetProductByIdQuery(int ProductId) : IRequest<Result<ProductDetailDto>>;
