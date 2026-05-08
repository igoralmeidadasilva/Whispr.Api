namespace Whispr.Presentation.Web.Core.Dtos;

public sealed record AuthTokenDto
{
    public required Guid UserId { get; init; }
    public required string UserEmail { get; init; }
    public required string UserName { get; init; }
    public required string AccessToken { get; init; }
    public required DateTimeOffset AccessTokenExpirationAtUtc { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTimeOffset RefreshTokenExpirationAtUtc { get; init; }
}