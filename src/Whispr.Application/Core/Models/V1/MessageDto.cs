namespace Whispr.Application.Core.Models.V1;

public sealed record MessageDto
{
    public Guid Id { get; init; }
    public Guid SenderId { get; init; }
    public string? Content { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
}