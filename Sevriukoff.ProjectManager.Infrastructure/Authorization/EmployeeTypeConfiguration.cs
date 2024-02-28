using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sevriukoff.ProjectManager.Infrastructure.Authorization;

public class EmployeeTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email)
            .IsUnique();
        
        builder.Property(x => x.FirstName)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.Patronymic)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();
    }
}