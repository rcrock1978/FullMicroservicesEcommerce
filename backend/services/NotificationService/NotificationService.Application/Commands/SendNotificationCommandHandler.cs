using AutoMapper;
using MediatR;
using NotificationService.Application.Services;
using NotificationService.Contracts.Dtos;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace NotificationService.Application.Commands;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Result<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SendNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IEmailService emailService,
        ISmsService smsService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _emailService = emailService;
        _smsService = smsService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<NotificationDto>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.NotificationDto;
            var notification = new Notification(
                request.UserId,
                dto.Email,
                dto.Type,
                dto.Subject,
                dto.Message,
                dto.PhoneNumber,
                dto.TemplateId,
                dto.ReferenceType,
                dto.ReferenceId
            );

            if (dto.TemplateData != null)
            {
                foreach (var kvp in dto.TemplateData)
                {
                    notification.AddTemplateData(kvp.Key, kvp.Value);
                }
            }

            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send notification asynchronously
            _ = Task.Run(async () => await SendNotificationAsync(notification.Id, dto.Type, dto.Email, dto.Subject, dto.Message, dto.PhoneNumber), cancellationToken);

            var notificationDto = _mapper.Map<NotificationDto>(notification);
            return Result<NotificationDto>.Success(notificationDto);
        }
        catch (Exception ex)
        {
            return Result<NotificationDto>.Failure($"Failed to send notification: {ex.Message}");
        }
    }

    private async Task SendNotificationAsync(int notificationId, NotificationType type, string email, string subject, string message, string? phoneNumber)
    {
        try
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null) return;

            bool success = type switch
            {
                NotificationType.Email => await _emailService.SendEmailAsync(email, subject, message),
                NotificationType.SMS when !string.IsNullOrEmpty(phoneNumber) => await _smsService.SendSmsAsync(phoneNumber, message),
                _ => false
            };

            if (success)
            {
                notification.MarkAsSent();
            }
            else
            {
                notification.MarkAsFailed("Failed to send notification");
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.MarkAsFailed(ex.Message);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
