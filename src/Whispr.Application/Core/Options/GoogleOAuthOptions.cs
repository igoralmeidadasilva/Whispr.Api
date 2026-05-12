namespace Whispr.Application.Core.Options;

public sealed record GoogleOAuthOptions
{
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
}