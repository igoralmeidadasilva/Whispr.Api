using Whispr.SharedKernel.Pagination;

namespace Whispr.Domain.Core.Repositories;

public interface IReadOnlyRepository<TEntity>
{
    Task<PagedList<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}