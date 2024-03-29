﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sevriukoff.ProjectManager.Infrastructure.Entities.TypeConfigurations;

public class ProjectTaskTypeConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.Priority)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .IsRequired();
        
        builder.Property(x => x.Comment)
            .HasMaxLength(500)
            .IsRequired(false);
    }
}