using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whispr.Application.Commands.Users;
using Whispr.Presentation.Api.Core.Interfaces;
using Whispr.SharedKernel.Results;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Whispr.Presentation.Api.Endpoints;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IVersionedEndpointRouteBuilder builder)
    {
        // var api = builder.NewVersionedApi(nameof(UserEndpoints));
        var group = builder.MapGroup("/api/v{version:apiVersion}/users")
            .HasApiVersion(1)
            .WithTags("Users")
            .WithOpenApi();

        group.MapGet("/", GetAll)
           .WithName("GetAllUsers")
           .Produces(200);
        
        group.MapPost("/", Create)
            .WithName("CreateUser")
            .Produces(200);
    }

    private static async Task<IResult> Create(
        [FromServices] ISender sender,
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> response = await sender.Send(command, cancellationToken);
        if (response.IsFailure)
        {
            return Results.BadRequest(response.Errors);
        }
        return Results.Created();
    }
    
    private static IResult GetAll()
    {
        return Results.Ok(new { Message = "Buscou todos os usuários" });
    }
}