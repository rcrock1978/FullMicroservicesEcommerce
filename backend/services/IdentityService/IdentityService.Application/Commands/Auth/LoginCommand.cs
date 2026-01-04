using IdentityService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Commands.Auth;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<TokenDto>>;
