using LifeTrack.Core.Interfaces.Services.Mappers;
using LifeTrack.Core.Models.DTOs;
using LifeTrack.Core.Models.Entities;

namespace LifeTrack.Services.Mapper;

public class KanbanTaskMapper : IKanbanTaskMapper
{
    private readonly IKanbanCategoryMapper _categoryMapper;

    public KanbanTaskMapper(IKanbanCategoryMapper categoryMapper)
    {
        _categoryMapper = categoryMapper;
    }
    public KanbanTaskDTO Map(KanbanTaskEntity entity)
    {
        return new KanbanTaskDTO(
            Id: entity.Id,
            Title: entity.Title,
            DescriptionMarkdown: entity.DescriptionMarkdown,
            IsImportant: entity.IsImportant,
            IsCompleted: entity.IsCompleted,
            Category: _categoryMapper.Map(entity.Category),
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );
    }
}