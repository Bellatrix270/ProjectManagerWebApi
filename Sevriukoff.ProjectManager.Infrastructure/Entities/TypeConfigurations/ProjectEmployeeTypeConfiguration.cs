using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sevriukoff.ProjectManager.Infrastructure.Entities.TypeConfigurations;

public class ProjectEmployeeTypeConfiguration : IEntityTypeConfiguration<ProjectEmployee>
{
    public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
    {
        builder.HasKey(x => new {x.ProjectId, x.EmployeeId});
    }
}