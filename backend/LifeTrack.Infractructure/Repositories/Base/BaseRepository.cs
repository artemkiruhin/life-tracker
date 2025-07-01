using System.Linq.Expressions;
using LifeTrack.Core.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace LifeTrack.Infractructure.Repositories.Base;

public class BaseRepository<TEntity>(AppDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    protected AppDbContext Context { get; } = context;
    protected DbSet<TEntity> DbSet { get; } = context.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await DbSet.FindAsync(keyValues: [id], ct);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
    {
        return await DbSet.AsNoTracking().ToListAsync(ct);
    }

    public async Task<IEnumerable<TEntity>> FindRangeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync(ct);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
    {
        return await DbSet.AsNoTracking().Where(predicate).FirstOrDefaultAsync(ct);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct)
    {
        var addResult = await DbSet.AddAsync(entity, ct);
        return addResult.Entity;
    }

    public TEntity? Update(TEntity entity)
    {
        var updateResult = DbSet.Update(entity);
        return updateResult.Entity;
    }

    public TEntity Delete(TEntity entity)
    {
        var deleteResult = DbSet.Remove(entity);
        return deleteResult.Entity;
    }
}