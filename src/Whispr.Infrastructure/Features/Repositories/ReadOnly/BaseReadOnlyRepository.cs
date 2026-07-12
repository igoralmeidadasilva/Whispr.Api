using Microsoft.EntityFrameworkCore;
using Whispr.Domain.Core.Repositories;
using Whispr.Domain.Features.Entities;
using Whispr.Infrastructure.Core.Data.Context;
using Whispr.Infrastructure.Core.Extensions;
using Whispr.SharedKernel.Pagination;

namespace Whispr.Infrastructure.Features.Repositories.ReadOnly;

internal abstract class BaseReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext Context;

    protected BaseReadOnlyRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Context
            .Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<PagedList<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await Context
            .Set<TEntity>()
            .AsNoTracking()
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context
            .Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context
            .Set<TEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }
}