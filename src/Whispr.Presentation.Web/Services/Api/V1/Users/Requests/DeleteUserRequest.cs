namespace Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

public sealed record DeleteUserRequest
{
    public required Guid UserId { get; init; }
}