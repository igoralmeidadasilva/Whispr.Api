using Whispr.Application.Core.Dtos.V1;

namespace Whispr.Application.Features.V1.Messages.Events.MessageCreated;

public sealed record MessageCreatedNotification : INotification
{
    public Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public IEnumerable<MessageAttachmentDto>? Attachments { get;init; }
}