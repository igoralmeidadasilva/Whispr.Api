using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whispr.Application.Core.Models.V1;
using Whispr.Application.Features.V1.Auth.Commands.GoogleLogin;
using Whispr.Application.Features.V1.Auth.Commands.Login;
using Whispr.Application.Features.V1.Auth.Commands.Logout;
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

        group.MapPost(Constants.Routes.Auth.LoginWithGoogle, LoginWithGoogle)
            .WithName("LoginWithGoogle")
            .Produces<AuthTokenDto>(StatusCodes.Status302Found)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        group.MapPost(Constants.Routes.Auth.Refresh, Refresh)
            .WithName("Refresh")
            .Produces<AuthTokenDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost(Constants.Routes.Auth.Logout, Logout)
            .WithName("Logout")
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

    private static async Task<IResult> LoginWithGoogle(
        [FromBody] LoginWithGoogleCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<AuthTokenDto> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }

    private static async Task<IResult> Logout(
        [FromBody] LogoutCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }
}