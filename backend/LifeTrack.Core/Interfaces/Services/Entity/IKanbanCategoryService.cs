using LifeTrack.Core.Models.Contracts;
using LifeTrack.Core.Models.Contracts.Create;
using LifeTrack.Core.Models.Contracts.Update;
using LifeTrack.Core.Models.DTOs;

namespace LifeTrack.Core.Interfaces.Services.Entity;

public interface IKanbanCategoryService
{
    Task<Result<KanbanCategoryDTO>> GetCategoryById (Guid categoryId, Guid userId, CancellationToken ct);
    Task<Result<List<KanbanCategoryDTO>>> GetAllCategories (Guid userId, CancellationToken ct);
    Task<Result<KanbanCategoryDTO>> Create (KanbanCategoryCreateContract request, CancellationToken ct);
    Task<Result<KanbanCategoryDTO>> Update (KanbanCategoryUpdateContract request, CancellationToken ct);
    Task<Result<KanbanCategoryDTO>> Delete (Guid categoryId, Guid userId, CancellationToken ct);
}