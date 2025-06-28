namespace LifeTrack.Core.Models.Entities;

public class KanbanTaskEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? DescriptionMarkdown { get; set; }
    public bool IsImportant { get; set; }
    public bool IsCompleted { get; set; }
    public Guid TaskCategoryId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}