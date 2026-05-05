namespace Whispr.Domain.Features.Models;

public sealed record TokenModel
{
    public required string AccessToken { get; init; }
    public required DateTimeOffset AccessTokenExpirationAtUtc { get; init; }
}