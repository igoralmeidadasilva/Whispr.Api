namespace Whispr.Application.Core.Dtos.V1;

public sealed record MessageDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public UserDto? User { get; init; }
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public IEnumerable<MessageAttachmentDto>? Attachments { get; init; }
}