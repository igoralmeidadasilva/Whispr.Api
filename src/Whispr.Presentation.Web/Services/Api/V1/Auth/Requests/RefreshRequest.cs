namespace Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

public sealed record RefreshRequest
{
    public required string ExpiredAccessToken { get; init; }
    public required string RefreshToken { get; init; }
}