using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whispr.Application.Core.Models.V1;
using Whispr.Application.Features.V1.Users.Commands.Create;
using Whispr.Application.Features.V1.Users.Commands.Delete;
using Whispr.Application.Features.V1.Users.Commands.PasswordRecoveryCode;
using Whispr.Application.Features.V1.Users.Commands.Update;
using Whispr.Application.Features.V1.Users.Queries.GetById;
using Whispr.Application.Features.V1.Users.Queries.GetUsers;
using Whispr.Presentation.Api.Core.Extensions;
using Whispr.Presentation.Api.Core.Factories;
using Whispr.Presentation.Api.Core.Interfaces;
using Whispr.Presentation.Api.Core.Models;
using Whispr.SharedKernel.Pagination;
using Whispr.SharedKernel.Results;

namespace Whispr.Presentation.Api.Endpoints.V1;

public sealed class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(Constants.Routes.User.Root)
            .HasApiVersion(1)
            .WithTags("Users")
            .WithOpenApi()
            .RequireRateLimiting(Constants.Settings.RateLimiter);

        group.MapGet(Constants.Routes.User.GetAll, GetAll)
            .WithName("GetUsers")
            .RequireAuthorization()
            .Produces<PagedModel<UserDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);

        group.MapGet(Constants.Routes.User.GetById, GetById)
            .WithName("GetUserById")
            .RequireAuthorization()
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost(Constants.Routes.User.Create, Create)
            .WithName("CreateUser")
            .AllowAnonymous()
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

        group.MapPut(Constants.Routes.User.Update, Update)
            .WithName("UpdateUser")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

        group.MapDelete(Constants.Routes.User.Delete, Delete)
            .WithName("DeleteUser")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost(Constants.Routes.User.PasswordRecoveryCode, PasswordRecoveryCode)
            .WithName("Password Recovery Code")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPatch(Constants.Routes.User.PasswordReset, PasswordReset)
            .WithName("Password Reset")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAll(
        [FromServices] ISender sender,
        [FromServices] PagedModelFactory pagedLinkFactory,
        [FromQuery] int pageNumber = Application.Constants.Pagination.DefaultPageNumber,
        [FromQuery] int pageSize = Application.Constants.Pagination.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        Result<PagedList<UserDto>> response = await sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return Results.Problem(response.Error.ToProblemDetails());
        }

        PagedModel<UserDto> pagedModelResponse = pagedLinkFactory.Create(response.Value!);
        return Results.Ok(pagedModelResponse);
    }

    private static async Task<IResult> GetById(
        [AsParameters] GetUserByIdQuery query,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<UserDto> response = await sender.Send(query, cancellationToken);
        return response.Match(Results.Ok);
    }

    private static async Task<IResult> Create(
        [FromBody] CreateUserCommand command,
        [FromServices] ISender sender, 
        CancellationToken cancellationToken = default)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Created);
    }

    private static async Task<IResult> Update(
        [FromRoute] Guid userId,
        [FromBody] UpdateUserCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        command = command with { UserId = userId };
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.NoContent);
    }

    private static async Task<IResult> Delete(
        [AsParameters] DeleteUserCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.NoContent);
    }

    private static async Task<IResult> PasswordRecoveryCode(
        [FromBody] PasswordRecoveryCodeCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }

    private static async Task<IResult> PasswordReset(
        [FromBody] PasswordResetCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Ok);
    }
}