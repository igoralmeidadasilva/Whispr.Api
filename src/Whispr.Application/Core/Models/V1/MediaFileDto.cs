namespace Whispr.Application.Core.Models.V1;

public sealed record MediaFileDto
{
    public required byte[] Value { get; set; }
    public required string ContentType { get; set; }
    public required string Name { get; set; }
    public bool IsVideo => ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase);
    public bool IsImage => ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
}