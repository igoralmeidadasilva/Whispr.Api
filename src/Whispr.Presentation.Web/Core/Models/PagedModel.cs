namespace Whispr.Presentation.Web.Core.Models;

public sealed record PagedModel<T>
{
    public IEnumerable<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public string? Next { get; init; }
    public string? Previous { get; init; }
}