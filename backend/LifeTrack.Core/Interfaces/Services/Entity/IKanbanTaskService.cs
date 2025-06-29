using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Specific;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;

namespace LifeTrack.Core.Interfaces.Services.Entity;

public interface IKanbanTaskService
{
    Task<KanbanTaskDTO> GetTaskById (Guid taskId, Guid userId, CancellationToken ct);
    Task<List<KanbanTaskDTO>> GetAllCategories (Guid userId, CancellationToken ct);
    Task<KanbanTaskDTO> Create (KanbanTaskCreateContract request, CancellationToken ct);
    Task<KanbanTaskDTO> Update (KanbanTaskUpdateContract request, CancellationToken ct);
    Task<KanbanTaskDTO> Delete (Guid taskId, Guid userId, CancellationToken ct);
    Task<KanbanTaskDTO> RemoveCategory (RemoveCategoryContract request, CancellationToken ct);
    Task<KanbanTaskDTO> ChangeCategory (ChangeCategoryContract request, CancellationToken ct);
}