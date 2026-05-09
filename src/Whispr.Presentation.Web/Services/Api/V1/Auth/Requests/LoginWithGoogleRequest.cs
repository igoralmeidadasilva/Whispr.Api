namespace Whispr.Presentation.Web.Services.Api.V1.Auth.Requests;

public sealed record LoginWithGoogleRequest
{
    public required string IdToken { get; init; }
}