using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whispr.Application.Core.Services;
using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Core.Services;
using Whispr.Domain.Features.Entities.Messages;
using Whispr.Domain.Features.Entities.PasswordResetTokens;
using Whispr.Domain.Features.Entities.RefreshTokens;
using Whispr.Domain.Features.Entities.Users;
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
                .ConfigureServices()
                .ConfigureAzureServices(configuration);

        return services;
    }

    private static IServiceCollection ConfigureAzureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients((clientBuilder) =>
        {
            clientBuilder.AddBlobServiceClient(configuration.GetConnectionString("StorageConnection"));
        });

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
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserPersistenceRepository, UserPersistenceRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();

        services.AddScoped<IRefreshTokenPersistenceRepository, RefreshTokenPersistenceRepository>();
        services.AddScoped<IRefreshTokenReadOnlyRepository, RefreshTokenReadOnlyRepository>();

        services.AddScoped<IRefreshTokenPersistenceRepository, RefreshTokenPersistenceRepository>();
        services.AddScoped<IRefreshTokenReadOnlyRepository, RefreshTokenReadOnlyRepository>();

        services.AddScoped<IPasswordResetTokenPersistenceRepository, PasswordResetTokenPersistenceRepository>();
        services.AddScoped<IPasswordResetTokenReadOnlyRepository, PasswordResetTokenReadOnlyRepository>();

        services.AddScoped<IMessagePersistenceRepository, MessagePersistenceRepository>();
        services.AddScoped<IMessageReadOnlyRepository, MessageReadOnlyRepository>();

        return services;
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<ITokenHasherService, TokenHasherService>();
        services.AddScoped<IAuthTokenService, AuthTokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IStorageService, AzureBlobStorageService>();

        return services;
    }
}