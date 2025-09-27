using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.RateLimiting;
using Whispr.Domain.Entities;
using Whispr.Infrastructure.Context;
using Whispr.Presentation.Api.Core.Configurations;

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
                .AddConfigurationOptions(configuration)
                .ConfigureApiHealthCheck(configuration)
                .AddSwaggerGen();
        return services;
    }
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        return services;
    }

    public static IServiceCollection ConfigureAspVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        int majorVersion = configuration.GetValue<int>("Version");
        int minorVersion = 0;
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version")
            );
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

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
                foreach (var description in descriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }

    public static void UserCustomHealthCheck(this WebApplication app)
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
}