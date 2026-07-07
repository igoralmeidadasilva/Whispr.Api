namespace Whispr.Application.Core.Dtos.V1;

public sealed record AuthTokenDto
{
    public required string AccessToken { get; init; }
    public required DateTimeOffset AccessTokenExpirationAtUtc { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTimeOffset RefreshTokenExpirationAtUtc { get; init; }
}