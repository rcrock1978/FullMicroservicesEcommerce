using MediatR;
using Shared.Common.Application;

namespace ProductService.Application.Commands.Products;

public record DeleteProductCommand(int ProductId) : IRequest<Result>;
