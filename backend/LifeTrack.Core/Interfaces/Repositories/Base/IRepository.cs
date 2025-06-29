using System.Linq.Expressions;

namespace LifeTrack.Core.Interfaces.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<TEntity>> FindRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken ct);
    TEntity? Update(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity, CancellationToken ct);
}