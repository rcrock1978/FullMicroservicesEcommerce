using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;
using Shared.Common.Domain;

namespace PaymentService.Domain.Repositories;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<Payment?> GetByStripePaymentIntentIdAsync(string paymentIntentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
}
