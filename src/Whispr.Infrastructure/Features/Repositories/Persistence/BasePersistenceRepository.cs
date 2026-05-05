using Whispr.Domain.Core.Repositories;
using Whispr.Domain.Features.Entities;
using Whispr.Infrastructure.Core.Data.Context;

namespace Whispr.Infrastructure.Features.Repositories.Persistence;

public abstract class BasePersistenceRepository<TEntity> : IPersistenceRepository<TEntity> where TEntity : Entity
{
    protected readonly ApplicationDbContext Context;

    protected BasePersistenceRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public void Insert(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }

    public void InsertRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }
}