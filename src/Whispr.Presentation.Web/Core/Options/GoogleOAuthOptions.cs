namespace Whispr.Presentation.Web.Core.Options;

public sealed record GoogleOAuthOptions
{
    public required string ClientId { get; init; }
}