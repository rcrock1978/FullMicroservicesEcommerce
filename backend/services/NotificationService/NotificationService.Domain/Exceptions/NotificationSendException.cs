namespace NotificationService.Domain.Exceptions;

public class NotificationSendException : Exception
{
    public NotificationSendException(string message) : base(message)
    {
    }

    public NotificationSendException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
