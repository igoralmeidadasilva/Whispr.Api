using Whispr.Domain.Core.Services;
using Whispr.SharedKernel.Guard;

namespace Whispr.Infrastructure.Features.Services;

internal sealed class PasswordHasherService : IPasswordHasherService
{
    private const int DEFAULT_WORK_FACTOR = 12;

    public string Hash(string password, int workFactor = DEFAULT_WORK_FACTOR)
    {
        Ensure.NotEmpty(password, "Password cannot be empty.", nameof(password));

        var salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

        return hashedPassword;
    }

    public bool Verify(string password, string hash)
    {
        Ensure.NotEmpty(password, "Password cannot be empty.", nameof(password));
        Ensure.NotEmpty(hash, "Hash cannot be empty.", nameof(hash));

        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}