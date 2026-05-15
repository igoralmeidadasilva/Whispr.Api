using Whispr.Domain.Features.Entities.Users;
using Whispr.SharedKernel.Guard;

namespace Whispr.Domain.Features.Entities.PasswordResetTokens;

public sealed class PasswordResetToken : Entity
{
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public TokenHash TokenHash { get; private set; } = null!;
    public DateTimeOffset ExpirationAtUtc { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? UsedAtUtc { get; private set; }
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationAtUtc;

    public PasswordResetToken() { } // ORM Constructor

    public PasswordResetToken(
        Guid userId,
        TokenHash tokenHash)
    {
        Ensure.NotNullOrDefault(userId, "User ID cannot be empty.", nameof(userId));
        Ensure.NotNullOrDefault(tokenHash, "TokenHash hash cannot be empty.", nameof(tokenHash));

        UserId = userId;
        TokenHash = tokenHash;
        ExpirationAtUtc = DateTimeOffset.UtcNow.AddMinutes(Constants.Constraints.PasswordResetToken.ExpirationMinutes);
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkAsUsed()
    {
        Ensure.IsFalse(IsExpired, "Cannot use an expired token.", nameof(IsExpired));
        UsedAtUtc = DateTimeOffset.UtcNow;
    }
}