using MediatR;
using ProductService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace ProductService.Application.Commands.Products;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, Result>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStockCommandHandler(
        IInventoryRepository inventoryRepository,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        if (inventory == null)
        {
            return Result.Failure($"Inventory for product {request.ProductId} not found");
        }

        try
        {
            if (request.Operation.Equals("Add", StringComparison.OrdinalIgnoreCase))
            {
                inventory.AddStock(request.Quantity);
            }
            else if (request.Operation.Equals("Remove", StringComparison.OrdinalIgnoreCase))
            {
                inventory.RemoveStock(request.Quantity);
            }
            else
            {
                return Result.Failure("Operation must be 'Add' or 'Remove'");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
