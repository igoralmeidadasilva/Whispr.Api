using Asp.Versioning.Builder;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.RateLimiting;
using Whispr.Domain.Features.Entities.User;
using Whispr.Infrastructure.Core.Data.Context;
using Whispr.Presentation.Api.Core;
using Whispr.Presentation.Api.Core.Configurations;
using Whispr.Presentation.Api.Core.Factories;
using Whispr.Presentation.Api.Core.Interfaces;

namespace Whispr.Presentation.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer()
            .ConfigureCors()
            .ConfigureRateLimiter()
            .ConfigureAspVersioning()
            .ConfigureOptions()
            .ConfigureApiHealthCheck(configuration)
            .ConfigureSwaggerGen()
            .ConfigureFactories();
        return services;
    }
    
    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        return services;
    }

    private static IServiceCollection ConfigureFactories(this IServiceCollection services)
    {
        services.AddScoped<PagedModelFactory>();
        return services;
    }

    private static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        return services;
    }

    private static IServiceCollection ConfigureAspVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        })
        .EnableApiVersionBinding();
        return services;
    }

    private static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://localhost:7059", "http://localhost:5223")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
            });
        });
        return services;
    }

    private static IServiceCollection ConfigureRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(opts =>
        {
            opts.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            opts.AddPolicy(Constants.Settings.RateLimiter, httpContext =>
            {
                var partition = RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 24,
                        Window = TimeSpan.FromSeconds(12),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 2
                    });
                return partition;
            });
        });
        return services;
    }

    private static IServiceCollection ConfigureApiHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        
        services.AddHealthChecks().AddNpgSql(
            connectionString: connectionString,
            name: "PostgreSQL",
            tags: ["db", "tags"]);

        services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(5);
            options.MaximumHistoryEntriesPerEndpoint(10);
            options.AddHealthCheckEndpoint("WhispR.Api health checks", Constants.Routes.Shared.Health);
        })
        .AddPostgreSqlStorage(connectionString, options =>
        {
            options.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning);
            });
        });
        return services;
    }
    
    public static void UseCustomHealthCheck(this WebApplication app)
    {
        app.UseHealthChecks(Constants.Routes.Shared.Health, new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = Constants.Routes.Shared.Dashboard;
        });
    }

    public static void UseCustomSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();

                foreach ( var description in descriptions )
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }
    }
    
    private static void MapVersionedEndpoints(this IVersionedEndpointRouteBuilder builder)
    {
        var endpointTypes = typeof(IEndpoint).Assembly
            .GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(type)!;
            endpoint.MapEndpoint(builder);
        }
    }

    public static void MapEndpoints(this WebApplication app)
    {
        var versionedBuilder = app.NewVersionedApi();
        versionedBuilder.MapVersionedEndpoints();
    }
}