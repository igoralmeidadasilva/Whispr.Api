namespace Whispr.Presentation.Web.Core.Authentication;

public sealed class TokenProvider : ITokenProvider
{
    private string? _accessToken;
    private DateTimeOffset? _expiration;

    public string? GetAccessToken()
    {
        if (!_expiration.HasValue)
        {
            return null;
        }

        if (DateTimeOffset.UtcNow < _expiration.Value)
        {
            return _accessToken;
        }

        return null;
    }

    public void Clear()
    {
        _accessToken = null;
        _expiration = null;
    }

    public void SetAccessToken(string token, DateTimeOffset expiration)
    {
        _accessToken = token;
        _expiration = expiration;
    }
}