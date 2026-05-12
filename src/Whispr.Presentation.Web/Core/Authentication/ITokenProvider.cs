namespace Whispr.Presentation.Web.Core.Authentication;

public interface ITokenProvider
{
    string? GetAccessToken();
    void SetAccessToken(string token, DateTimeOffset expiration);
    void Clear();
}