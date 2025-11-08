using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Whispr.Application.Core.Models.V1;
using Whispr.Application.Features.V1.Users.Commands.Create;
using Whispr.Application.Features.V1.Users.Queries.GetById;
using Whispr.Application.Features.V1.Users.Queries.GetUsers;
using Whispr.Presentation.Api.Core.Extensions;
using Whispr.Presentation.Api.Core.Interfaces;
using Whispr.SharedKernel.Results;

namespace Whispr.Presentation.Api.Endpoints;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/v{version:apiVersion}/users")
            .HasApiVersion(1)
            .WithTags("Users")
            .WithOpenApi()
            .RequireRateLimiting(Constants.Settings.RateLimiter);

        group.MapGet("/", GetAll)
           .WithName("GetUsers");

        group.MapGet("/{Id:Guid}", GetById)
           .WithName("GetUserById");

        group.MapPost("/", Create)
            .WithName("CreateUser")
            .Produces((int)HttpStatusCode.Created)
            .Produces<ProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces<ProblemDetails>((int)HttpStatusCode.Conflict);

        group.MapPut("/{UserId:Guid}", Update)
            .WithName("UpdateUser");

        group.MapDelete("/{UserId:Guid}", Delete)
            .WithName("DeleteUser");
    }

    private static async Task<IResult> GetAll(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        Result<IEnumerable<UserDto>> response = await sender.Send(new GetUsersQuery(), cancellationToken);
        return response.Match(Results.Ok, Results.NotFound);
    }

    private static async Task<IResult> GetById(
        [FromServices] ISender sender,
        [AsParameters] GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        Result<UserDto> response = await sender.Send(query, cancellationToken);
        return response.Match(Results.Ok, Results.NotFound);
    }

    private static async Task<IResult> Create(
        [FromServices] ISender sender,
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Created, Results.BadRequest);
    }

    private static IResult Update()
    {
        return Results.Ok(new { Message = "Buscou todos os usuários" });
    }
    
    private static IResult Delete()
    {
        return Results.Ok(new { Message = "Buscou todos os usuários" });
    }
}