using System.Net;
using System.Text.Json;
using IdentityService.Domain.Exceptions;
using FluentValidation;

namespace IdentityService.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var (statusCode, message, errors) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                "Validation failed",
                validationEx.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }).ToList()
            ),
            InvalidCredentialsException => (
                HttpStatusCode.Unauthorized,
                exception.Message,
                null
            ),
            EmailAlreadyExistsException => (
                HttpStatusCode.Conflict,
                exception.Message,
                null
            ),
            InvalidRefreshTokenException => (
                HttpStatusCode.Unauthorized,
                exception.Message,
                null
            ),
            UserNotActiveException => (
                HttpStatusCode.Forbidden,
                exception.Message,
                null
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "An internal server error occurred",
                null
            )
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            message,
            errors
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
