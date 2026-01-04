using CartService.Domain.Entities;
using CartService.Domain.Repositories;
using CartService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Infrastructure.Persistence;

namespace CartService.Infrastructure.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(CartDbContext context) : base(context)
    {
    }

    public async Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Cart>()
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<Cart?> GetByUserIdWithItemsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Cart>()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<Cart>> GetExpiredCartsAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Cart>()
            .Include(c => c.Items)
            .Where(c => c.ExpiresAt.HasValue && c.ExpiresAt.Value < DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Cart>()
            .AnyAsync(c => c.UserId == userId, cancellationToken);
    }
}
