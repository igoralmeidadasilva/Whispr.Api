using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whispr.Application.Features.V1.Auth.Commands.Login;
using Whispr.Domain.Features.Models;
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
            .WithTags("Auth")
            .WithOpenApi()
            .RequireRateLimiting(Constants.Settings.RateLimiter);

        group.MapPost(Constants.Routes.Auth.Login, Login)
            .WithName("Login")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> Login(
        [FromBody] LoginCommand command,
        [FromServices] ISender sender, 
        CancellationToken cancellationToken = default)
    {
        Result<TokenModel> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }
}