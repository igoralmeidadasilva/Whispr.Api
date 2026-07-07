using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whispr.Application.Core.Dtos.V1;
using Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;
using Whispr.Application.Features.V1.Auth.Commands.Login;
using Whispr.Application.Features.V1.Auth.Commands.Logout;
using Whispr.Application.Features.V1.Auth.Commands.Refresh;
using Whispr.Domain.Features.Models;
using Whispr.Presentation.Api.Core.Extensions;
using Whispr.Presentation.Api.Core.Interfaces;
using Whispr.SharedKernel.Results;

namespace Whispr.Presentation.Api.Endpoints.V1;

public sealed class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IVersionedEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup(Constants.Routes.Auth.Root)
            .HasApiVersion(1)
            .WithTags("Authentication")
            .WithOpenApi()
            .RequireRateLimiting(Constants.Settings.RateLimiter);

        group.MapPost(Constants.Routes.Auth.Login, Login)
            .WithName("Login")
            .Produces<AuthTokenDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .AllowAnonymous();

        group.MapPost(Constants.Routes.Auth.LoginWithGoogle, LoginWithGoogle)
            .WithName("LoginWithGoogle")
            .Produces<AuthTokenDto>(StatusCodes.Status302Found)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost(Constants.Routes.Auth.Refresh, Refresh)
            .WithName("Refresh")
            .Produces<AuthTokenDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .AllowAnonymous();

        group.MapPost(Constants.Routes.Auth.Logout, Logout)
            .WithName("Logout")
            .Produces<AuthTokenDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Login(
        [FromBody] LoginCommand command,
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        Result<AuthTokenDto> response = await sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            context.Response.Cookies.Delete(Constants.Settings.CookieRefreshToken);
            return Results.Problem(response.Error.ToProblemDetails());
        }

        AppendRefreshTokenCookie(context, response.Value!.RefreshToken, response.Value!.RefreshTokenExpirationAtUtc);

        TokenModel token = new()
        {
            Token = response.Value.AccessToken,
            TokenExpirationAtUtc = response.Value.AccessTokenExpirationAtUtc
        };
        return Results.Ok(token);
    }

    private static async Task<IResult> Refresh(
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        string? refreshToken = context.Request.Cookies[Constants.Settings.CookieRefreshToken];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Results.Unauthorized();
        }

        RefreshCommand command = new()
        {
            RefreshToken = refreshToken
        };
        Result<AuthTokenDto> response = await sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            context.Response.Cookies.Delete(Constants.Settings.CookieRefreshToken);
            return Results.Problem(response.Error.ToProblemDetails());
        }

        AppendRefreshTokenCookie(context, response.Value!.RefreshToken, response.Value!.RefreshTokenExpirationAtUtc);

        TokenModel token = new()
        {
            Token = response.Value.AccessToken,
            TokenExpirationAtUtc = response.Value.AccessTokenExpirationAtUtc
        };
        return Results.Ok(token);
    }

    private static async Task<IResult> LoginWithGoogle(
        [FromBody] LoginWithGoogleCommand command,
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        Result<AuthTokenDto> response = await sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            context.Response.Cookies.Delete(Constants.Settings.CookieRefreshToken);
            return Results.Problem(response.Error.ToProblemDetails());
        }

        AppendRefreshTokenCookie(context, response.Value!.RefreshToken, response.Value!.RefreshTokenExpirationAtUtc);

        TokenModel token = new()
        {
            Token = response.Value.AccessToken,
            TokenExpirationAtUtc = response.Value.AccessTokenExpirationAtUtc
        };
        return Results.Ok(token);
    }

    private static async Task<IResult> Logout(
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        string? refreshToken = context.Request.Cookies[Constants.Settings.CookieRefreshToken];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Results.Ok();
        }

        LogoutCommand command = new()
        {
            RefreshToken = refreshToken
        };
        Result<Unit> response = await sender.Send(command, cancellationToken);

        if (response.IsFailure)
        {
            return Results.Problem(response.Error.ToProblemDetails());
        }

        context.Response.Cookies.Delete(Constants.Settings.CookieRefreshToken, new CookieOptions
        {
            Path = GetCookiePath(context)
        });

        return Results.Ok();
    }

    private static void AppendRefreshTokenCookie(
        HttpContext context,
        string refreshToken,
        DateTimeOffset expiresAt)
    {
        string path = GetCookiePath(context);
        context.Response.Cookies.Append(Constants.Settings.CookieRefreshToken, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = expiresAt,
            Path = path,
            IsEssential = true
        });
    }

    private static string GetCookiePath(HttpContext context)
    {
        string version = context.GetRequestedApiVersion()?.ToString() ?? "1";
        return $"/api/v{version}/auth";
    }
}