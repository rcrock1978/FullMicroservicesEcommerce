using IdentityService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Queries.Users;

public record GetUserByEmailQuery(string Email) : IRequest<Result<UserDto>>;
