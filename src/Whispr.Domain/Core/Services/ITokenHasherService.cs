namespace Whispr.Domain.Core.Services;

public interface ITokenHasherService
{
    string Hash(string rawToken);
    bool Verify(string rawToken, string hash);
}
