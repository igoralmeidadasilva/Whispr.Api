using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Whispr.Application.Core.Behaviors;
using Whispr.SharedKernel.Results.Factories;

namespace Whispr.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMediatR(configuration)
                .ConfigureValidators()
                .ConfigureFactories();

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
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection ConfigureFactories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IResultFactory<>), typeof(ResultFactory<>));
        return services;
    }
}