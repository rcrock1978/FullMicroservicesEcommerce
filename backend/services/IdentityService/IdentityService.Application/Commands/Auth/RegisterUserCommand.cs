using IdentityService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Commands.Auth;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string? PhoneNumber
) : IRequest<Result<UserDto>>;
