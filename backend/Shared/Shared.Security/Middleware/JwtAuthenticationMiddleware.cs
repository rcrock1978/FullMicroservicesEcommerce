using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Security.Middleware;

/// <summary>
/// Middleware for extracting and validating JWT authentication
/// </summary>
public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = ExtractTokenFromHeader(context);

        if (!string.IsNullOrEmpty(token))
        {
            // Token validation is handled by JWT Bearer authentication scheme
            // This middleware is for additional custom logic if needed
            
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                
                // Add custom claims or context data
                context.Items["UserId"] = jwtToken.Subject;
                context.Items["TokenExpiration"] = jwtToken.ValidTo;
            }
        }

        await _next(context);
    }

    private static string? ExtractTokenFromHeader(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader))
            return null;

        const string bearerPrefix = "Bearer ";
        if (authHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return authHeader[bearerPrefix.Length..];
        }

        return null;
    }
}
