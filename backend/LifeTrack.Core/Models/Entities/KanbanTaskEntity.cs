namespace LifeTrack.Core.Models.Entities;

public class KanbanTaskEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? DescriptionMarkdown { get; set; }
    public bool IsImportant { get; set; }
    public bool IsCompleted { get; set; }
    public Guid? TaskCategoryId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public virtual KanbanTaskCategoryEntity Category { get; set; } = null!;
    public virtual UserEntity User { get; set; } = null!;

    public static KanbanTaskEntity Create(string title, string? descriptionMarkdown, bool isImportant, Guid? taskCategoryId, Guid userId) =>
        new()
        {
            Id = Guid.NewGuid(),
            Title = title,
            DescriptionMarkdown = descriptionMarkdown,
            IsImportant = isImportant,
            IsCompleted = false,
            TaskCategoryId = taskCategoryId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
}