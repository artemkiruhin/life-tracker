using LifeTrack.Core.Interfaces;
using LifeTrack.Core.Interfaces.Repositories;

namespace LifeTrack.Infractructure;

public class UnitOfWork(
    AppDbContext context,
    IUserRepository userRepository,
    IKanbanTaskRepository kanbanTaskRepository,
    IKanbanCategoryRepository kanbanCategoryRepository)
    : IUnitOfWork
{
    private AppDbContext Context { get; } = context;
    public IUserRepository UserRepository { get; } = userRepository;
    public IKanbanTaskRepository KanbanTaskRepository { get; } = kanbanTaskRepository;
    public IKanbanCategoryRepository KanbanCategoryRepository { get; } = kanbanCategoryRepository;


    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await Context.SaveChangesAsync(ct);
    }

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        await Context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct)
    {
        await Context.Database.CommitTransactionAsync(ct);
    }

    public async Task RollbackTransactionAsync(CancellationToken ct)
    {
        await Context.Database.RollbackTransactionAsync(ct);
    }
}