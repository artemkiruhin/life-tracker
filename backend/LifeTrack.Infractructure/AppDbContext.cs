using LifeTrack.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeTrack.Infractructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<KanbanTaskCategoryEntity> KanbanTaskCategories { get; set; }
    public DbSet<KanbanTaskEntity> KanbanTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(opt =>
        {
            opt.ToTable("users");
            opt.HasKey(x => x.Id);
            opt.Property(x => x.Id).IsRequired();
            opt.HasIndex(x => x.Id).IsUnique();

            opt.Property(x => x.Username).IsRequired().HasMaxLength(15);
            opt.HasIndex(x => x.Username).IsUnique();

            opt.Property(x => x.PasswordHash).IsRequired();

            opt.Property(x => x.Email).IsRequired().HasMaxLength(50);
            opt.HasIndex(x => x.Email).IsUnique();

            opt.Property(x => x.CreatedAt).IsRequired();

            opt
                .HasMany(x => x.KanbanCategories)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            opt
                .HasMany(x => x.KanbanTasks)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<KanbanTaskCategoryEntity>(opt =>
        {
            opt.ToTable("kanban_task_categories");
            opt.HasKey(x => x.Id);
            opt.Property(x => x.Id).IsRequired();
            opt.HasIndex(x => x.Id).IsUnique();

            opt.Property(x => x.Name).IsRequired().HasMaxLength(20);
            opt.HasIndex(x => x.Name).IsUnique();

            opt.Property(x => x.UserId).IsRequired();
            opt.Property(x => x.CreatedAt).IsRequired();
            
            opt
                .HasOne(x => x.User)
                .WithMany(x => x.KanbanCategories)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            opt
                .HasMany(x => x.Tasks)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.TaskCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<KanbanTaskEntity>(opt =>
        {
            opt.ToTable("kanban_tasks");
            opt.HasKey(x => x.Id);
            opt.Property(x => x.Id).IsRequired();
            opt.HasIndex(x => x.Id).IsUnique();

            opt.Property(x => x.Title).IsRequired().HasMaxLength(20);
            opt.Property(x => x.IsImportant).IsRequired();
            opt.Property(x => x.IsCompleted).IsRequired();
            opt.Property(x => x.UserId).IsRequired();
            opt.Property(x => x.CreatedAt).IsRequired();
            
            opt
                .HasOne(x => x.Category)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.TaskCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            opt
                .HasOne(x => x.User)
                .WithMany(x => x.KanbanTasks)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
}