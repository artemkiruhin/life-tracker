namespace LifeTrack.Core.Models.Entities;

public class KanbanTaskCategoryEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}