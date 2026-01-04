using ProductService.Domain.Entities;
using Shared.Common.Domain;

namespace ProductService.Domain.Repositories;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<Inventory?> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inventory>> GetLowStockItemsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Inventory>> GetOutOfStockItemsAsync(CancellationToken cancellationToken = default);
}
