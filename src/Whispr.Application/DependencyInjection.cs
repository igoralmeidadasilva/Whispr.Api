using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whispr.Application.Core.Behaviors;

namespace Whispr.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAppication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMediatR(configuration)
                .ConfigureValidators();
        return services;
    }

    public static IServiceCollection ConfigureMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
               .AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
               .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        return services;
    }

    public static IServiceCollection ConfigureValidators(this IServiceCollection services)
    {
        return services;
    }
}
