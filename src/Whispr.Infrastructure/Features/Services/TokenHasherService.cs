using System.Security.Cryptography;
using System.Text;
using Whispr.Domain.Core.Services;

namespace Whispr.Infrastructure.Features.Services;

internal sealed class TokenHasherService : ITokenHasherService
{
    public string Hash(string rawToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public bool Verify(string rawToken, string hash)
    {
        var computed = Hash(rawToken);
        return computed == hash;
    }
}