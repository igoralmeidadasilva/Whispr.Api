using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Whispr.Application.Core.Models.V1;
using Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;
using Whispr.Application.Features.V1.Auth.Commands.Login;
using Whispr.Application.Features.V1.Auth.Commands.Refresh;
using Whispr.Presentation.Api.Core.Extensions;
using Whispr.Presentation.Api.Core.Interfaces;
using Whispr.SharedKernel.Results;

namespace Whispr.Presentation.Api.Endpoints.V1;

public sealed class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(Constants.Routes.Auth.Root)
            .HasApiVersion(1)
            .WithTags("Authentication")
            .WithOpenApi()
            .RequireRateLimiting(Constants.Settings.RateLimiter);

        group.MapPost(Constants.Routes.Auth.Login, Login)
            .WithName("Login")
            .Produces<AuthTokenDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet(Constants.Routes.Auth.LoginWithGoogle, LoginWithGoogle)
            .WithName("LoginWithGoogle")
            .Produces<AuthTokenDto>(StatusCodes.Status302Found)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapGet(Constants.Routes.Auth.GoogleCallback, GoogleCallback)
            .WithName("GoogleCallback")
            .Produces<AuthTokenDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .AllowAnonymous();

        group.MapPost(Constants.Routes.Auth.Refresh, Refresh)
            .WithName("Refresh")
            .Produces<AuthTokenDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> Login(
        [FromBody] LoginCommand command,
        [FromServices] ISender sender, 
        CancellationToken cancellationToken = default)
    {
        Result<AuthTokenDto> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }

    private static async Task<IResult> Refresh(
        [FromBody] RefreshCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<AuthTokenDto> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }

    private static IResult LoginWithGoogle(HttpContext httpContext, LinkGenerator linkGenerator)
    {
        string? callbackUrl = linkGenerator.GetUriByName(
            httpContext,
            "GoogleCallback");

        var properties = new AuthenticationProperties
        {
            RedirectUri = callbackUrl
        };

        return Results.Challenge(properties, [GoogleDefaults.AuthenticationScheme]);
    }

    private static async Task<IResult> GoogleCallback(
        HttpContext httpContext,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        AuthenticateResult result = await httpContext.AuthenticateAsync(
            GoogleDefaults.AuthenticationScheme);

        if (!result.Succeeded || result.Principal is null)
            return Results.Problem(
                detail: "Google authentication failed or was cancelled.",
                statusCode: StatusCodes.Status401Unauthorized);

        ClaimsPrincipal principal = result.Principal;

        string? email    = principal.FindFirstValue(ClaimTypes.Email);
        string? name     = principal.FindFirstValue(ClaimTypes.Name);
        string? googleId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(googleId))
            return Results.Problem(
                detail: "Required claims (email, id) were not returned by Google.",
                statusCode: StatusCodes.Status400BadRequest);

        var command = new GoogleLoginCommand
        {
            GoogleId = googleId,
            Email = email,
            Name = name!
        };

        Result<AuthTokenDto> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }
}