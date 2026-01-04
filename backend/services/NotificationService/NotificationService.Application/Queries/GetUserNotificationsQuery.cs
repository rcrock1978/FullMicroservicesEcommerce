using MediatR;
using NotificationService.Contracts.Dtos;
using Shared.Common.Application;

namespace NotificationService.Application.Queries;

public record GetUserNotificationsQuery(int UserId) : IRequest<Result<IEnumerable<NotificationDto>>>;
