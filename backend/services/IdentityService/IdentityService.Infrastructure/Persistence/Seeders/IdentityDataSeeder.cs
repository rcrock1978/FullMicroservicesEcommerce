using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Security.Hashing;

namespace IdentityService.Infrastructure.Persistence.Seeders;

public class IdentityDataSeeder
{
    private readonly IdentityDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<IdentityDataSeeder> _logger;

    public IdentityDataSeeder(
        IdentityDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<IdentityDataSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Ensure database is created
            await _context.Database.MigrateAsync();

            // Seed admin user if not exists
            await SeedAdminUserAsync();

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedAdminUserAsync()
    {
        var adminEmail = "admin@ecommerce.com";
        
        if (await _context.Users.AnyAsync(u => u.Email == adminEmail))
        {
            _logger.LogInformation("Admin user already exists");
            return;
        }

        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole == null)
        {
            _logger.LogWarning("Admin role not found. Skipping admin user creation");
            return;
        }

        var passwordHash = _passwordHasher.HashPassword("Admin@123");

        var adminUser = User.Create(
            "Admin",
            "User",
            adminEmail,
            passwordHash,
            "+1234567890"
        );

        adminUser.VerifyEmail();
        adminUser.VerifyPhone();
        adminUser.AddRole(adminRole);

        await _context.Users.AddAsync(adminUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Admin user created successfully with email: {Email}", adminEmail);
    }
}
