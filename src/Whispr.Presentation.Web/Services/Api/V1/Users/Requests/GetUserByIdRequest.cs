namespace Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

public sealed record GetUserByIdRequest
{
    public required Guid UserId { get; init; }
}