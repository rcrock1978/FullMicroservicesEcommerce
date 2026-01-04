using CartService.Domain.Repositories;
using CartService.Infrastructure.Caching;
using CartService.Infrastructure.Data;
using CartService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common.Domain;
using StackExchange.Redis;

namespace CartService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CartDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Repositories
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Redis Cache
        var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configurationOptions = ConfigurationOptions.Parse(redisConnection);
            return ConnectionMultiplexer.Connect(configurationOptions);
        });
        services.AddScoped<IRedisCacheService, RedisCacheService>();

        return services;
    }
}
