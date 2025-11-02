using Microsoft.AspNetCore.Identity;

namespace Whispr.Domain.Entities;

public sealed class User : IdentityUser
{
    public ICollection<Message> Messages { get; set; } = [];

    public User()
    {
        Id = Guid.CreateVersion7().ToString();
    }
}