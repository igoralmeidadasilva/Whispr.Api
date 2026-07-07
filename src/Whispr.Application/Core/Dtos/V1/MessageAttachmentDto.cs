namespace Whispr.Application.Core.Dtos.V1;

public sealed record MessageAttachmentDto
{
    public Guid Id { get; init; }
    public Guid MessageId { get; init; }
    public string? SasUri { get; init; }
}