using System.Linq.Expressions;
using Whispr.Domain.Core.Enums;
using Whispr.Domain.Features.Entities.Messages;

namespace Whispr.Domain.Features.Models;

public sealed record MessagePaginatedSearchParameters
{
    public bool WithAttachments { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public SortDirection SortDirection { get; init; } = SortDirection.Ascending;
}