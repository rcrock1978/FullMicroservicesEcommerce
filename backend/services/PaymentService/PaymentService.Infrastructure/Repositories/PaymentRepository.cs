using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Repositories;
using PaymentService.Infrastructure.Data;
using Shared.Common.Infrastructure.Persistence;

namespace PaymentService.Infrastructure.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(PaymentDbContext context) : base(context)
    {
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.OrderId == orderId, cancellationToken);
    }

    public async Task<Payment?> GetByStripePaymentIntentIdAsync(string paymentIntentId, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.StripePaymentIntentId == paymentIntentId, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(p => p.OrderId == orderId, cancellationToken);
    }
}
