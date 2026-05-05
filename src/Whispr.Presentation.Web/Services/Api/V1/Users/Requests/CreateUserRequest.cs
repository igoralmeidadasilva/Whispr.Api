namespace Whispr.Presentation.Web.Services.Api.V1.Users.Requests;

public sealed record CreateUserRequest
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}