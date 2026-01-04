using AutoMapper;
using MediatR;
using NotificationService.Contracts.Dtos;
using NotificationService.Domain.Repositories;
using Shared.Common.Application;

namespace NotificationService.Application.Queries;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, Result<IEnumerable<NotificationDto>>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<NotificationDto>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var notificationDtos = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        return Result<IEnumerable<NotificationDto>>.Success(notificationDtos);
    }
}
