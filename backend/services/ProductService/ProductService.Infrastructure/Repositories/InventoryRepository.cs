using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Persistence;
using Shared.Common.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Repositories;

public class InventoryRepository : Repository<Inventory>, IInventoryRepository
{
    private readonly ProductDbContext _context;

    public InventoryRepository(ProductDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Inventory?> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.ProductId == productId, cancellationToken);
    }

    public async Task<IEnumerable<Inventory>> GetLowStockItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .Where(i => i.QuantityOnHand - i.ReservedQuantity <= i.ReorderLevel && 
                       i.QuantityOnHand - i.ReservedQuantity > 0)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inventory>> GetOutOfStockItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .Where(i => i.QuantityOnHand - i.ReservedQuantity <= 0)
            .ToListAsync(cancellationToken);
    }
}
