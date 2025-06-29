namespace LifeTrack.Core.Models.DTOs;

public record KanbanTaskDTO(
    Guid Id,
    string Title,
    string? DescriptionMarkdown,
    bool IsImportant,
    bool IsCompleted,
    KanbanCategoryDTO Category,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);