using LifeTrack.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeTrack.Infractructure.Configurations;

public class KanbanCategoryConfiguration : IEntityTypeConfiguration<KanbanTaskCategoryEntity>
{
    public void Configure(EntityTypeBuilder<KanbanTaskCategoryEntity> builder)
    {
        builder.ToTable("kanban_task_categories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
            
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.KanbanCategories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasMany(x => x.Tasks)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.TaskCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}