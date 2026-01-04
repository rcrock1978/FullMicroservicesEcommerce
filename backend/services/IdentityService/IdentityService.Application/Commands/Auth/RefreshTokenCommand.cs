using IdentityService.Contracts.DTOs;
using MediatR;
using Shared.Common.Application;

namespace IdentityService.Application.Commands.Auth;

public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<Result<TokenDto>>;
