using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Entities.TypeConfigurations;

namespace Sevriukoff.ProjectManager.Infrastructure;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> opt) : base(opt) { }

    public ProjectDbContext() { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=project.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new EmployeeTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectTaskTypeConfiguration());
    }
}
