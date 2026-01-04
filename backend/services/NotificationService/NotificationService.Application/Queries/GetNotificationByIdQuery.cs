using MediatR;
using NotificationService.Contracts.Dtos;
using Shared.Common.Application;

namespace NotificationService.Application.Queries;

public record GetNotificationByIdQuery(int NotificationId) : IRequest<Result<NotificationDto>>;
