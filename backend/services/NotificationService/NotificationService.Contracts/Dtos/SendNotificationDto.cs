using NotificationService.Domain.Enums;

namespace NotificationService.Contracts.Dtos;

public record SendNotificationDto(
    string Email,
    NotificationType Type,
    string Subject,
    string Message,
    string? PhoneNumber = null,
    string? TemplateId = null,
    Dictionary<string, string>? TemplateData = null,
    string? ReferenceType = null,
    int? ReferenceId = null
);
