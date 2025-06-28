namespace LifeTrack.Core.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public required string SecurityPin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<KanbanTaskEntity> KanbanTasks { get; set; } = [];
    public virtual ICollection<KanbanTaskCategoryEntity> KanbanCategories { get; set; } = [];

    public static UserEntity Create(string username, string passwordHash, string email, string securityPin) =>
        new()
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = passwordHash,
            Email = email,
            SecurityPin = securityPin,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
}