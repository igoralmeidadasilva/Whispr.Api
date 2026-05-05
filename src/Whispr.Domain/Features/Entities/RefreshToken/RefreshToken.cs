namespace Whispr.Domain.Features.Entities.RefreshToken;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; private set; }
    public User.User? User { get; private set; } = null;
    public string Token { get; private set; } = string.Empty;
    public DateTimeOffset ExpirationAtUtc { get; private set; }

    public RefreshToken() {} // ORM Constructor
    
    public RefreshToken(Guid userId, string token, DateTimeOffset expirationAtUtc) : base()
    {
        UserId = userId;
        Token = token;
        ExpirationAtUtc = expirationAtUtc;
    }

    public bool IsExpired()
    {
        return DateTimeOffset.UtcNow >= ExpirationAtUtc;
    }
}