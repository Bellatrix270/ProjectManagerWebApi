using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sevriukoff.ProjectManager.Infrastructure.Entities.TypeConfigurations;

public class ProjectTypeConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.CustomerCompany)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.ExecutorCompany)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(x => x.Employees)
            .WithMany(x => x.Projects)
            .UsingEntity(x => x.ToTable("EmployeeProject"));

        builder.HasOne(x => x.Manager)
            .WithMany()
            .HasForeignKey(x => x.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Tasks)
            .WithOne()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}