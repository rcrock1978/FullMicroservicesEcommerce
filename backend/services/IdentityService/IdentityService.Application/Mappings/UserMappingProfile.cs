using AutoMapper;
using IdentityService.Contracts.DTOs;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => 
                src.UserRoles.Select(ur => ur.Role.Name).ToList()));
    }
}
