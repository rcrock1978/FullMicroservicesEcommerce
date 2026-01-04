using MediatR;
using ProductService.Contracts.DTOs;
using Shared.Common.Application;

namespace ProductService.Application.Commands.Products;

public record UpdateProductCommand(
    int ProductId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string Status
) : IRequest<Result<ProductDto>>;
