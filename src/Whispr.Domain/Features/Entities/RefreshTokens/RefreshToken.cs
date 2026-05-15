using Whispr.Domain.Features.Entities.Users;
using Whispr.SharedKernel.Guard;

namespace Whispr.Domain.Features.Entities.RefreshTokens;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; private set; }
    public User? User { get; private set; } = null;
    public TokenHash TokenHash { get; private set; } = null!;
    public DateTimeOffset ExpirationAtUtc { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? RevokedAtUtc { get; private set; }
    public bool IsRevoked => RevokedAtUtc.HasValue;
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationAtUtc;
    public bool IsActive => !IsRevoked && !IsExpired;

    public RefreshToken() {} // ORM Constructor
    
    public RefreshToken(Guid userId, TokenHash tokenHash, DateTimeOffset expirationAtUtc) : base()
    {
        Ensure.NotNullOrDefault(userId, "User ID cannot be empty.", nameof(userId));
        Ensure.NotNullOrDefault(tokenHash, "Token Hash cannot be empty.", nameof(tokenHash));
        Ensure.NotNullOrDefault(expirationAtUtc, "Expiration date cannot be null or default.", nameof(expirationAtUtc));

        UserId = userId;
        TokenHash = tokenHash;
        ExpirationAtUtc = expirationAtUtc;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Revoke()
    {
        RevokedAtUtc = DateTimeOffset.UtcNow;
    }
}