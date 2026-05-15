using Whispr.Domain.Core.Interfaces;
using Whispr.SharedKernel.Guard;

namespace Whispr.Domain.Features.Entities.Users;

public sealed class User : Entity, ISoftDeletable, IAuditable
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public Password PasswordHash { get; private set; } = null!;
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public User() {} // ORM Constructor

    public User(string name, string email, Password password) : base()
    {
        Ensure.NotEmpty(name, "Name cannot be empty.", nameof(name));
        Ensure.NotEmpty(email, "Email cannot be empty.", nameof(email));

        Name = name;
        Email = email;
        PasswordHash = password;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string name, string email)
    {
        Ensure.NotEmpty(name, "Name cannot be empty.", nameof(name));
        Ensure.NotEmpty(email, "Email cannot be empty.", nameof(email));

        Name = name;
        Email = email;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAtUtc = null;
    }
}