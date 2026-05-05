namespace Whispr.Domain.Core.Repositories;

public interface IPersistenceRepository<TEntity>
{
    void Insert(TEntity entity);
    void InsertRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}