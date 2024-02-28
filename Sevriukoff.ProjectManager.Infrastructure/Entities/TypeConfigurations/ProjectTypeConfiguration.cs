using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;

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
        
        builder.HasMany(x => x.Tasks)
            .WithOne()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.Employees);
    }
}