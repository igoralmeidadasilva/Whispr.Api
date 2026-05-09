namespace Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

public sealed record LogoutRequest
{
    public required string RefreshToken { get; init; }
}