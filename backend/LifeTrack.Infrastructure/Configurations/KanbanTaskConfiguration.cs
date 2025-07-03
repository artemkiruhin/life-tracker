using LifeTrack.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeTrack.Infractructure.Configurations;

public class KanbanTaskConfiguration : IEntityTypeConfiguration<KanbanTaskEntity>
{
    public void Configure(EntityTypeBuilder<KanbanTaskEntity> builder)
    {
        builder.ToTable("kanban_tasks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Title).IsRequired().HasMaxLength(20);
        builder.Property(x => x.IsImportant).IsRequired();
        builder.Property(x => x.IsCompleted).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
            
        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.TaskCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.KanbanTasks)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}