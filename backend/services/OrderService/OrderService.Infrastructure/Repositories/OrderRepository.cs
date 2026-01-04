using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<Order?> GetByIdWithItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByUserIdWithItemsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .Include(o => o.Items)
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .AnyAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }
}
