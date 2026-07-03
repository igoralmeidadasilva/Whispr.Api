using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Features.Entities.Users;

namespace Whispr.Domain.Features.Entities.Messages;

public sealed class Message : Entity, IAuditable
{
    public Guid SenderId { get; private set; } // UserId
    public User? Sender { get; private set; }
    public string? Content { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public Message() {} // ORM Constructor

    public Message(Guid senderId, string? content) : base()
    {
        SenderId = senderId;
        Content = content;

        CreatedAtUtc = DateTime.UtcNow;
    }
}