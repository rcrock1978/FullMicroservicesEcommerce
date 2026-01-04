using NotificationService.Domain.Enums;
using NotificationService.Domain.Events;
using Shared.Common.Domain;

namespace NotificationService.Domain.Entities;

public class Notification : BaseEntity
{
    public int UserId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public NotificationType Type { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public NotificationStatus Status { get; private set; }
    public DateTime? SentAt { get; private set; }
    public string? FailureReason { get; private set; }
    public int RetryCount { get; private set; }
    
    // Additional metadata
    public string? TemplateId { get; private set; }
    public Dictionary<string, string> TemplateData { get; private set; } = new();
    public string? ReferenceType { get; private set; } // e.g., "Order", "Payment"
    public int? ReferenceId { get; private set; }

    private Notification() { } // EF Core

    public Notification(
        int userId,
        string email,
        NotificationType type,
        string subject,
        string message,
        string? phoneNumber = null,
        string? templateId = null,
        string? referenceType = null,
        int? referenceId = null)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be empty.", nameof(subject));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.", nameof(message));

        UserId = userId;
        Email = email;
        PhoneNumber = phoneNumber;
        Type = type;
        Subject = subject;
        Message = message;
        Status = NotificationStatus.Pending;
        TemplateId = templateId;
        ReferenceType = referenceType;
        ReferenceId = referenceId;
        RetryCount = 0;

        AddDomainEvent(new NotificationCreatedEvent(Id, UserId, Type, Subject));
    }

    public void MarkAsSent()
    {
        if (Status == NotificationStatus.Sent)
            throw new InvalidOperationException("Notification already sent");

        Status = NotificationStatus.Sent;
        SentAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new NotificationSentEvent(Id, UserId, Type, SentAt.Value));
    }

    public void MarkAsFailed(string failureReason)
    {
        Status = NotificationStatus.Failed;
        FailureReason = failureReason;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new NotificationFailedEvent(Id, UserId, Type, FailureReason));
    }

    public void IncrementRetryCount()
    {
        RetryCount++;
        Status = NotificationStatus.Pending;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddTemplateData(string key, string value)
    {
        TemplateData[key] = value;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanRetry(int maxRetries = 3)
    {
        return RetryCount < maxRetries && Status == NotificationStatus.Failed;
    }
}
