using LifeTrack.Core.Interfaces.Repositories;

namespace LifeTrack.Core.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
    Task BeginTransactionAsync(CancellationToken ct);
    Task CommitTransactionAsync(CancellationToken ct);
    Task RollbackTransactionAsync(CancellationToken ct);
    
    public IUserRepository UserRepository { get; }
    public IKanbanTaskRepository KanbanTaskRepository { get; }
    public IKanbanCategoryRepository KanbanCategoryRepository { get; }
}