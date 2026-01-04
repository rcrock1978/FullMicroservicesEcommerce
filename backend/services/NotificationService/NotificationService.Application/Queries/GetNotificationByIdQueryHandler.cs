using AutoMapper;
using MediatR;
using NotificationService.Contracts.Dtos;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.Repositories;
using Shared.Common.Application;

namespace NotificationService.Application.Queries;

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, Result<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public GetNotificationByIdQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<Result<NotificationDto>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken);
        
        if (notification == null)
        {
            return Result<NotificationDto>.Failure(new NotificationNotFoundException(request.NotificationId).Message);
        }

        var notificationDto = _mapper.Map<NotificationDto>(notification);
        return Result<NotificationDto>.Success(notificationDto);
    }
}
