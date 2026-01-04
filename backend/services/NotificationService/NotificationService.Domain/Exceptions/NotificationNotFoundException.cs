using Shared.Common.Domain.Exceptions;

namespace NotificationService.Domain.Exceptions;

public class NotificationNotFoundException : NotFoundException
{
    public NotificationNotFoundException(int notificationId)
        : base("Notification", notificationId, $"Notification with ID '{notificationId}' was not found.")
    {
    }
}
