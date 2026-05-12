namespace Whispr.Domain.Features.Models;

public sealed record TokenModel
{
    public required string Token { get; init; }
    public required DateTimeOffset TokenExpirationAtUtc { get; init; }
}