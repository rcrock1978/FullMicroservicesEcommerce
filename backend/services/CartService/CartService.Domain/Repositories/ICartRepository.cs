using CartService.Domain.Entities;
using Shared.Common.Domain;

namespace CartService.Domain.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Cart?> GetByUserIdWithItemsAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Cart>> GetExpiredCartsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
