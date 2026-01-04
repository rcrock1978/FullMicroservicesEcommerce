using IdentityService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Queries.Users;

public record GetUserByIdQuery(int UserId) : IRequest<Result<UserDto>>;
