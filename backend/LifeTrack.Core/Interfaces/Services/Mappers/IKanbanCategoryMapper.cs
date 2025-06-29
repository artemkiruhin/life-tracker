using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Core.Interfaces.Services.Mappers;

public interface IKanbanCategoryMapper
{
    KanbanCategoryDTO Map (KanbanTaskCategoryEntity entity);
}