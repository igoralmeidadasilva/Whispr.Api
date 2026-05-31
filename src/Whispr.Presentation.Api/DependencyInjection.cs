using Asp.Versioning.Builder;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Threading.RateLimiting;
using Whispr.Application.Core.Options;
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
            .ConfigureOptions(configuration)
            .ConfigureApiHealthCheck(configuration)
            .ConfigureSwaggerGen()
            .ConfigureFactories()
            .ConfigureSecurity(configuration);

        return services;
    }
    
    private static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddOptions<JwtAuthenticationOptions>()
            .Bind(configuration.GetSection(nameof(JwtAuthenticationOptions)))
            .ValidateOnStart();

        services.AddOptions<GoogleOAuthOptions>()
            .Bind(configuration.GetSection(nameof(GoogleOAuthOptions)))
            .ValidateOnStart();

        services.AddOptions<EmailOptions>()
            .Bind(configuration.GetSection(nameof(EmailOptions)))
            .ValidateOnStart();

        return services;
    }

    private static IServiceCollection ConfigureFactories(this IServiceCollection services)
    {
        services.AddScoped<PagedModelFactory>();

        return services;
    }

    private static IServiceCollection ConfigureSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtAuthenticationOptions)).Get<JwtAuthenticationOptions>();
        var googleOptions = configuration.GetSection(nameof(GoogleOAuthOptions)).Get<GoogleOAuthOptions>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.RequireHttpsMetadata = true;
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions!.Issuer,
                ValidAudience = jwtOptions!.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions!.Key)),
            };
        })
        .AddGoogle(options =>
        {
            options.ClientId = googleOptions!.ClientId;
            options.ClientSecret = googleOptions!.ClientSecret;
        });;

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
  
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

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

    public static void UseCustomSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
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