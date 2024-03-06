using System.ComponentModel.DataAnnotations;
using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Infrastructure.Entities;

public class ProjectTask : BaseEntity<Guid>
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public Guid CreatedById { get; set; }
    public Guid? AssignedToId { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public string Comment { get; set; }
    
    /// <summary>
    /// Целочисленное значение от 1 до 10
    /// </summary>
    public int Priority { get; set; }
}

/// <summary>
/// Status of task in project
/// </summary>
public enum ProjectTaskStatus
{
    /// <summary>
    /// ToDo
    /// </summary>
    ToDo,
    /// <summary>
    /// InProgress
    /// </summary>
    InProgress,
    /// <summary>
    /// Done
    /// </summary>
    Done
}