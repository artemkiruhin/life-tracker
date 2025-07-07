using LifeTrack.Core.Interfaces.Services.Mappers;
using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Services.Mapper;

public class KanbanCategoryMapper : IKanbanCategoryMapper
{
    public KanbanCategoryDTO Map(KanbanTaskCategoryEntity entity)
    {
        return new KanbanCategoryDTO(
            Id: entity.Id,
            Name: entity.Name,
            CreatedAt: entity.CreatedAt
        );
    }
}