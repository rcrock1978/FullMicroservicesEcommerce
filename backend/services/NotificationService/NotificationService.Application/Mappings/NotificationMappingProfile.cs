using AutoMapper;
using NotificationService.Contracts.Dtos;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Mappings;

public class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<Notification, NotificationDto>();
    }
}
