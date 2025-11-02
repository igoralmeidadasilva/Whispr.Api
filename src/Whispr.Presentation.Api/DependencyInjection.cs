using Asp.Versioning;
using Asp.Versioning.Builder;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.RateLimiting;
using Whispr.Domain.Entities;
using Whispr.Infrastructure.Context;
using Whispr.Presentation.Api.Core.Configurations;
using Whispr.Presentation.Api.Core.Interfaces;

namespace Whispr.Presentation.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        services.AddEndpointsApiExplorer()
            .ConfigureCors(configuration)
            .ConfigureRateLimiter(configuration)
            .ConfigureIdentityFramework(configuration)
            .ConfigureAspVersioning(configuration)
            .ConfigurationOptions(configuration)
            .ConfigureApiHealthCheck(configuration)
            .ConfigureSwaggerGen(configuration);
        return services;
    }
    public static IServiceCollection ConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        return services;
    }
    
    public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        return services;
    }

    public static IServiceCollection ConfigureAspVersioning(this IServiceCollection services, IConfiguration configuration)
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

    public static IServiceCollection ConfigureIdentityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
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

    public static IServiceCollection ConfigureRateLimiter(this IServiceCollection services, IConfiguration configuration)
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

    public static IServiceCollection ConfigureApiHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        services.AddHealthChecks()
            .AddNpgSql(connectionString: connectionString,
                       name: "PostgreSQL",
                       tags: ["db", "tags"]);

        services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(5);
            options.MaximumHistoryEntriesPerEndpoint(10);
            options.AddHealthCheckEndpoint("WhispR.Api health checks", Constants.Health.HealthUrl);
        })
        .AddInMemoryStorage();
        return services;
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

    public static void UseCustomHealthCheck(this WebApplication app)
    {
        app.UseHealthChecks(Constants.Health.HealthUrl, new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = Constants.Health.DashboardUrl;
        });
    }
   
    public static void MapVersionedEndpoints(this IVersionedEndpointRouteBuilder builder)
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