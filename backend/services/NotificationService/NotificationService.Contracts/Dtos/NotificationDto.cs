using NotificationService.Domain.Enums;

namespace NotificationService.Contracts.Dtos;

public record NotificationDto(
    int Id,
    int UserId,
    string Email,
    string? PhoneNumber,
    NotificationType Type,
    string Subject,
    string Message,
    NotificationStatus Status,
    DateTime? SentAt,
    string? FailureReason,
    int RetryCount,
    DateTime CreatedAt
);
