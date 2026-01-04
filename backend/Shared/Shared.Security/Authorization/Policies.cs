using Microsoft.AspNetCore.Authorization;

namespace Shared.Security.Authorization;

/// <summary>
/// Custom authorization policies for the application
/// </summary>
public static class Policies
{
    public const string RequireAdminRole = "RequireAdminRole";
    public const string RequireCustomerRole = "RequireCustomerRole";
    public const string RequireSellerRole = "RequireSellerRole";
    public const string RequireEmailVerified = "RequireEmailVerified";

    public static void AddCustomPolicies(AuthorizationOptions options)
    {
        // Role-based policies
        options.AddPolicy(RequireAdminRole, policy =>
            policy.RequireRole("Admin"));

        options.AddPolicy(RequireCustomerRole, policy =>
            policy.RequireRole("Customer", "Admin"));

        options.AddPolicy(RequireSellerRole, policy =>
            policy.RequireRole("Seller", "Admin"));

        // Claim-based policies
        options.AddPolicy(RequireEmailVerified, policy =>
            policy.RequireClaim("email_verified", "true"));
    }
}

/// <summary>
/// Authorization requirement for minimum age
/// </summary>
public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }

    public MinimumAgeRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}

/// <summary>
/// Authorization handler for minimum age requirement
/// </summary>
public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var dateOfBirthClaim = context.User.FindFirst(c => c.Type == "date_of_birth");

        if (dateOfBirthClaim == null)
        {
            return Task.CompletedTask;
        }

        if (DateTime.TryParse(dateOfBirthClaim.Value, out var dateOfBirth))
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
