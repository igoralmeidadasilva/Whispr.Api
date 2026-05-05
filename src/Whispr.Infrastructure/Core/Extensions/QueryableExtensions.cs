using Microsoft.EntityFrameworkCore;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Infrastructure.Core.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        int totalCount = await queryable.CountAsync(cancellationToken);
 
        List<T> items = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
 
        return new PagedList<T>(items, totalCount, pageNumber, pageSize);
    }
}