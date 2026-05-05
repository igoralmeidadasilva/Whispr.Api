using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.User;
using Whispr.Infrastructure.Core.Data.Context;
using Whispr.Infrastructure.Core.Interceptors;
using Whispr.Infrastructure.Features.Repositories;
using Whispr.Infrastructure.Features.Repositories.Persistence;
using Whispr.Infrastructure.Features.Repositories.ReadOnly;
using Whispr.Infrastructure.Features.Services;

namespace Whispr.Infrastructure;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration)
                .ConfigureRepositories()
                .ConfigureServices();
        return services;
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddSingleton<SoftDeleteInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString);
            options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
        });

        return services;
    }

    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUniteOfWork, UnitOfWork>();

        services.AddScoped<IUserPersistenceRepository, UserPersistenceRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();

        return services;
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IAuthTokenService, AuthTokenService>();

        return services;
    }
}