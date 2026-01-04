using IdentityService.Contracts.DTOs;
using IdentityService.Domain.Exceptions;
using IdentityService.Domain.Repositories;
using MediatR;
using Shared.Common.Application;
using Shared.Security.Authentication;
using Shared.Security.Hashing;

namespace IdentityService.Application.Commands.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Get user with roles
        var user = await _userRepository.GetByEmailWithRolesAsync(request.Email, cancellationToken);
        
        if (user == null)
        {
            return Result<TokenDto>.Failure("Invalid email or password");
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result<TokenDto>.Failure("Invalid email or password");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return Result<TokenDto>.Failure("User account is not active");
        }

        // Update last login
        user.UpdateLastLogin();

        // Generate tokens
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Store refresh token
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.AddRefreshToken(refreshToken, refreshTokenExpiry);

        var tokenDto = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };

        return Result<TokenDto>.Success(tokenDto);
    }
}
