using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whispr.Application.Core.Models.V1;
using Whispr.Application.Features.V1.Users.Commands.Create;
using Whispr.Application.Features.V1.Users.Commands.Delete;
using Whispr.Application.Features.V1.Users.Commands.Update;
using Whispr.Application.Features.V1.Users.Queries.GetById;
using Whispr.Application.Features.V1.Users.Queries.GetUsers;
using Whispr.Presentation.Api.Core;
using Whispr.Presentation.Api.Core.Extensions;
using Whispr.Presentation.Api.Core.Interfaces;
using Whispr.SharedKernel.Results;

namespace Whispr.Presentation.Api.Endpoints.V1;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(Routes.User.Root)
            .HasApiVersion(1)
            .WithTags("Users")
            .WithOpenApi()
            .RequireRateLimiting(Constants.Settings.RateLimiter);

        group.MapGet(Routes.User.GetAll, GetAll)
            .WithName("GetUsers")
            .Produces(StatusCodes.Status200OK);

        group.MapGet(Routes.User.GetById, GetById)
            .WithName("GetUserById")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost(Routes.User.Create, Create)
            .WithName("CreateUser")
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

        group.MapPut(Routes.User.Update, Update)
            .WithName("UpdateUser")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

        group.MapDelete(Routes.User.Delete, Delete)
            .WithName("DeleteUser")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAll(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        Result<IEnumerable<UserDto>> response = await sender.Send(new GetUsersQuery(), cancellationToken);
        return response.Match(Results.Ok);
    }

    private static async Task<IResult> GetById(
        [FromServices] ISender sender,
        [AsParameters] GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        Result<UserDto> response = await sender.Send(query, cancellationToken);
        return response.Match(Results.Ok);
    }

    private static async Task<IResult> Create(
        [FromServices] ISender sender,
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Created);
    }

    private static async Task<IResult> Update(
        [FromServices] ISender sender,
        [FromBody] UpdateUserCommand command,
        Guid userId,
        CancellationToken cancellationToken)
    {
        command = command with { UserId = userId };
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.NoContent);
    }
    
    private static async Task<IResult> Delete(
        [FromServices] ISender sender,
        [AsParameters] DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.NoContent);
    }
}