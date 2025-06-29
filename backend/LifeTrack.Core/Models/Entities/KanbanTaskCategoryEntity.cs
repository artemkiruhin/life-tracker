namespace LifeTrack.Core.Models.Entities;

public class KanbanTaskCategoryEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }

    public virtual ICollection<KanbanTaskEntity> Tasks { get; set; } = [];
    public virtual UserEntity User { get; set; } = null!;

    public static KanbanTaskCategoryEntity Create(string name, Guid userId) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };
}