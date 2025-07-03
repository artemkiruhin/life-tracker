using LifeTrack.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeTrack.Infractructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Username).IsRequired().HasMaxLength(15);
        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.PasswordHash).IsRequired();

        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder
            .HasMany(x => x.KanbanCategories)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasMany(x => x.KanbanTasks)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}