using IdentityService.Contracts.DTOs;
using IdentityService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;
using Shared.Security.Authentication;

namespace IdentityService.Application.Commands.Auth;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<TokenDto>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Find refresh token
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        
        if (refreshToken == null || !refreshToken.IsActive)
        {
            return Result<TokenDto>.Failure("Invalid or expired refresh token");
        }

        // Get user with roles
        var user = await _userRepository.GetByIdWithRolesAsync(refreshToken.UserId, cancellationToken);
        
        if (user == null || !user.IsActive)
        {
            return Result<TokenDto>.Failure("User not found or inactive");
        }

        // Revoke old refresh token
        refreshToken.Revoke();

        // Generate new tokens
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email, roles);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        // Store new refresh token
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.AddRefreshToken(newRefreshToken, refreshTokenExpiry);

        var tokenDto = new TokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };

        return Result<TokenDto>.Success(tokenDto);
    }
}
