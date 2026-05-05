namespace Whispr.Application.Core.Options;

public sealed record JwtAuthenticationOptions
{
    public required string Key { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int AccessTokenExpirationInMinutes { get; init; }
    public required int RefreshTokenExpirationInMinutes { get; init; }
}