using ProductService.Domain.Entities;
using Shared.Common.Domain;

namespace ProductService.Domain.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Review?> GetByProductAndUserAsync(int productId, int userId, CancellationToken cancellationToken = default);
}
