using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Infrastructure;

public class EmployeeTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Email)
            .IsUnique();
        
        builder.Property(x => x.Firstname)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.Lastname)
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