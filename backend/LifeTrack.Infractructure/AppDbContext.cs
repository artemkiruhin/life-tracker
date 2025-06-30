using LifeTrack.Core.Models.Entities;
using LifeTrack.Infractructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeTrack.Infractructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<KanbanTaskCategoryEntity> KanbanTaskCategories { get; set; }
    public DbSet<KanbanTaskEntity> KanbanTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new KanbanCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new KanbanTaskConfiguration());
    }
    
}