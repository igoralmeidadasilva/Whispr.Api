using Whispr.Domain.Core.Interfaces;
using Whispr.Domain.Features.Entities.Users;
using Whispr.SharedKernel.Guard;

namespace Whispr.Domain.Features.Entities.Messages;

public sealed class Message : Entity, IAuditable
{
    public Guid SenderId { get; private set; } // UserId
    public User? Sender { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public Message() {} // ORM Constructor

    public Message(Guid senderId, string content) : base()
    {
        Ensure.NotNullOrDefault(senderId, "User ID cannot be empty.", nameof(senderId));
        Ensure.NotEmpty(content, "Content cannot be empty.", nameof(content));

        SenderId = senderId;
        Content = content;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string content)
    {
        Ensure.NotEmpty(content, "Content cannot be empty.", nameof(content));
        
        Content = content;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}