using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;

namespace LifeTrack.Core.Interfaces.Services.Entity;

public interface IKanbanCategoryService
{
    Task<KanbanCategoryDTO> GetCategoryById (Guid categoryId, Guid userId, CancellationToken ct);
    Task<List<KanbanCategoryDTO>> GetAllCategories (Guid userId, CancellationToken ct);
    Task<KanbanCategoryDTO> Create (KanbanCategoryCreateContract request, CancellationToken ct);
    Task<KanbanCategoryDTO> Update (KanbanCategoryUpdateContract request, CancellationToken ct);
    Task<KanbanCategoryDTO> Delete (Guid categoryId, Guid userId, CancellationToken ct);
}