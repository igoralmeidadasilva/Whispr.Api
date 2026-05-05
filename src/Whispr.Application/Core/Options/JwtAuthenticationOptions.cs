namespace Whispr.Application.Core.Options;

public sealed record JwtAuthenticationOptions
{
    public required string Key { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int TokenExpirationInMinutes { get; init; }
}