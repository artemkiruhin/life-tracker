using LifeTrack.Core.Interfaces.Repositories;
using LifeTrack.Core.Models.Entities;
using LifeTrack.Infractructure.Repositories.Base;

namespace LifeTrack.Infractructure.Repositories;

public class KanbanCategoryRepository(AppDbContext context) : BaseRepository<KanbanTaskCategoryEntity>(context), IKanbanCategoryRepository
{
    
}