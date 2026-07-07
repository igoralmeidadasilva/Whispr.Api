using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Whispr.Application.Core.Interfaces;
using Whispr.Application.Core.Models.V1;
using Whispr.Application.Features.V1.Messages.Commands.Create;
using Whispr.Application.Features.V1.Messages.Commands.Delete;
using Whispr.Application.Features.V1.Messages.Commands.Update;
using Whispr.Application.Features.V1.Messages.Queries.GetById;
using Whispr.Application.Features.V1.Messages.Queries.GetMessages;
using Whispr.Presentation.Api.Core.Extensions;
using Whispr.Presentation.Api.Core.Factories;
using Whispr.Presentation.Api.Core.Interfaces;
using Whispr.Presentation.Api.Core.Models;
using Whispr.SharedKernel.Pagination;
using Whispr.SharedKernel.Results;

namespace Whispr.Presentation.Api.Endpoints.V1;

public sealed class MessageEndpoints : IEndpoint
{
    public void MapEndpoint(IVersionedEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup(Constants.Routes.Message.Root)
            .HasApiVersion(1)
            .WithTags("Messages")
            .WithOpenApi()
            .RequireRateLimiting(Constants.Settings.RateLimiter);
        
        group.MapGet(Constants.Routes.Message.GetAll, GetAll)
            .WithName("GetAllMessages")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet(Constants.Routes.Message.GetById, GetById)
            .WithName("GetMessageById")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
            
        group.MapPost(Constants.Routes.Message.Create, Create)
            .WithName("CreateMessage")
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .DisableAntiforgery();

        group.MapPut(Constants.Routes.Message.Update, Update)
            .WithName("UpdateMessage")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapDelete(Constants.Routes.Message.Delete, Delete)
            .WithName("DeleteMessage")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAll(
        [FromServices] ISender sender,
        [FromServices] PagedModelFactory pagedLinkFactory,
        [FromQuery] int pageNumber = Application.Constants.Pagination.DefaultPageNumber,
        [FromQuery] int pageSize = Application.Constants.Pagination.DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMessagesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        Result<PagedList<MessageDto>> response = await sender.Send(query, cancellationToken);

        if (response.IsFailure)
        {
            return Results.Problem(response.Error.ToProblemDetails());
        }

        PagedModel<MessageDto> pagedModelResponse = pagedLinkFactory.Create(response.Value!);
        return Results.Ok(pagedModelResponse);
    }

    private static async Task<IResult> GetById(
        [AsParameters] GetMessageByIdQuery query,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<MessageDto> response = await sender.Send(query, cancellationToken);
        return response.Match(Results.Ok);
    }

    private static async Task<IResult> Create(
        [FromForm] string content,
        [FromForm] IFormFileCollection attachments,
        [FromServices] ICurrentUserProvider currentUserProvider,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        CurrentUserDto? user = currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            return Results.Unauthorized();
        }

        CreateMessageCommand command = new()
        {
            UserId = user.Id,
            Content = content,
            Attachments = attachments
        };
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.Created);
    }

    private static async Task<IResult> Update(
        [FromRoute] Guid messageId,
        [FromBody] UpdateMessageCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        command = command with { MessageId = messageId };
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.NoContent);
    }

    private static async Task<IResult> Delete(
        [AsParameters] DeleteMessageCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken = default)
    {
        Result<Unit> response = await sender.Send(command, cancellationToken);
        return response.Match(Results.NoContent);
    }
}