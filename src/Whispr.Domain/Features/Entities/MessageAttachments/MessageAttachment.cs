using Whispr.Domain.Features.Entities.Messages;
using Whispr.SharedKernel.Guard;

namespace Whispr.Domain.Features.Entities.MessageAttachments;

public sealed class MessageAttachment : Entity
{
    public Guid MessageId { get; private set; }
    public Message? Message { get; private set; }
    public string StorageKey { get; private set; } = string.Empty;
    public string FileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long SizeBytes { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public MessageAttachment() { } // ORM Constructor

    public MessageAttachment(Guid messageId, string fileName, string contentType, long sizeBytes) : base()
    {
        Ensure.NotNullOrDefault(messageId, "Message ID cannot be empty.", nameof(messageId));
        Ensure.NotEmpty(fileName, "File name cannot be empty.", nameof(fileName));
        Ensure.NotEmpty(contentType, "Content type cannot be empty.", nameof(contentType));

        MessageId = messageId;
        StorageKey = $"attachments/{DateTime.UtcNow:yyyy/MM}/{base.Id}{Path.GetExtension(fileName)}";
        FileName = fileName;
        ContentType = contentType;
        SizeBytes = sizeBytes;
        CreatedAtUtc = DateTime.UtcNow;
    }
}