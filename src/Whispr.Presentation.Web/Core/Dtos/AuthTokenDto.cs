namespace Whispr.Presentation.Web.Core.Dtos;

public sealed record AuthTokenDto
{
    public required string Token { get; init; }
    public required DateTimeOffset TokenExpirationAtUtc { get; init; }
}