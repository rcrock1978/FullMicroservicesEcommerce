using MediatR;
using Shared.Common.Application;

namespace ProductService.Application.Commands.Products;

public record UpdateStockCommand(
    int ProductId,
    int Quantity,
    string Operation
) : IRequest<Result>;
