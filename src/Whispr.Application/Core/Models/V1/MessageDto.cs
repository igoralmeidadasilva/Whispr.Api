namespace Whispr.Application.Core.Models.V1;

public sealed record MessageDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string? UserName { get; init; }
    public string? Content { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
}