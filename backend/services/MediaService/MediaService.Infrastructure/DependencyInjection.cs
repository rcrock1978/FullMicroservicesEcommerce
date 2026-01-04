using Amazon.S3;
using MediaService.Application;
using MediaService.Domain.Repositories;
using MediaService.Infrastructure.Persistence;
using MediaService.Infrastructure.Persistence.Repositories;
using MediaService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common.Domain;

namespace MediaService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<MediaDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IMediaFileRepository, MediaFileRepository>();
        services.AddScoped<IImageMetadataRepository, ImageMetadataRepository>();

        // AWS S3
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<IFileStorageService, S3FileStorageService>();

        // Image Processing
        services.AddScoped<IImageProcessingService, ImageSharpProcessingService>();

        return services;
    }
}
