using Whispr.SharedKernel.Pagination;

namespace Whispr.Presentation.Api.Core.Models;

public sealed record PagedModel<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public string? Next { get; init; }
    public string? Previous { get; init; }
 
    public static PagedModel<T> From(PagedList<T> page, string? next, string? previous) => new()
    {
        Items = page.Items,
        TotalCount = page.TotalCount,
        Next = next,
        Previous = previous
    };
}