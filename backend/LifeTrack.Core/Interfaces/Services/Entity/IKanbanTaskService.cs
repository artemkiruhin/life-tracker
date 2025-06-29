using LifeTrack.Core.Models.Contracts;
using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Specific;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;

namespace LifeTrack.Core.Interfaces.Services.Entity;

public interface IKanbanTaskService
{
    Task<Result<KanbanTaskDTO>> GetTaskById (Guid taskId, Guid userId, CancellationToken ct);
    Task<Result<List<KanbanTaskDTO>>> GetAllCategories (Guid userId, CancellationToken ct);
    Task<Result<KanbanTaskDTO>> Create (KanbanTaskCreateContract request, CancellationToken ct);
    Task<Result<KanbanTaskDTO>> Update (KanbanTaskUpdateContract request, CancellationToken ct);
    Task<Result<KanbanTaskDTO>> Delete (Guid taskId, Guid userId, CancellationToken ct);
    Task<Result<KanbanTaskDTO>> RemoveCategory (RemoveCategoryContract request, CancellationToken ct);
    Task<Result<KanbanTaskDTO>> ChangeCategory (ChangeCategoryContract request, CancellationToken ct);
}