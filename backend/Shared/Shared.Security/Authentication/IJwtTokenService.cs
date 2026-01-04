using System.Security.Claims;

namespace Shared.Security.Authentication;

/// <summary>
/// Interface for JWT token generation and validation
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT access token for the specified user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="email">The user email</param>
    /// <param name="roles">The user roles</param>
    /// <param name="additionalClaims">Additional claims to include in the token</param>
    /// <returns>The generated JWT token</returns>
    string GenerateAccessToken(int userId, string email, IEnumerable<string> roles, IDictionary<string, string>? additionalClaims = null);

    /// <summary>
    /// Generates a refresh token
    /// </summary>
    /// <returns>The generated refresh token</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates a JWT token and returns the claims principal
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <returns>The claims principal if valid, null otherwise</returns>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    /// Gets the user ID from a token
    /// </summary>
    /// <param name="token">The JWT token</param>
    /// <returns>The user ID if found, null otherwise</returns>
    int? GetUserIdFromToken(string token);
}
