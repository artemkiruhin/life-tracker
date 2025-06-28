namespace LifeTrack.Core.Models.Entities;

public class KanbanTaskCategoryEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<KanbanTaskEntity> Tasks { get; set; } = [];

    public static KanbanTaskCategoryEntity Create(string name) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
}