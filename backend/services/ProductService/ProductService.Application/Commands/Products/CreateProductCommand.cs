using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Commands.Products;

public record CreateProductCommand(
    string Name,
    string Description,
    string Sku,
    decimal Price,
    string Currency,
    int CategoryId
) : IRequest<Result<ProductDto>>;
