using MediatR;
using NotificationService.Contracts.Dtos;
using Shared.Common.Application;

namespace NotificationService.Application.Commands;

public record SendNotificationCommand(
    int UserId,
    SendNotificationDto NotificationDto
) : IRequest<Result<NotificationDto>>;
