using System.ComponentModel.DataAnnotations;
using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Infrastructure.Entities;

public class ProjectTask : BaseEntity
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public int CreatedById { get; set; }
    public Employee CreatedBy { get; set; }
    public int? AssignedToId { get; set; }
    public Employee? AssignedTo { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public string Comment { get; set; }
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