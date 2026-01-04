using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using Shared.Common.Domain;

namespace NotificationService.Domain.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetByStatusAsync(NotificationStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetPendingNotificationsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetFailedNotificationsForRetryAsync(int maxRetries = 3, CancellationToken cancellationToken = default);
}
