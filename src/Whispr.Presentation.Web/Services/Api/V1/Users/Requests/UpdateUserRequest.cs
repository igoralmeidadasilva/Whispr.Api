using System.Text.Json.Serialization;

namespace Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

public sealed record UpdateUserRequest
{
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}