using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;

namespace IdentityService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // Add AutoMapper - manually register profiles
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddMaps(assembly);
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        // Add FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // Add pipeline behaviors from Shared.Common
        services.AddTransient(typeof(Shared.Common.Application.Behaviors.ValidationBehavior<,>));
        services.AddTransient(typeof(Shared.Common.Application.Behaviors.LoggingBehavior<,>));
        services.AddTransient(typeof(Shared.Common.Application.Behaviors.PerformanceBehavior<,>));

        return services;
    }
}
