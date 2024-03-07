using Sevriukoff.ProjectManager.Infrastructure.Entities;
#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.ProjectTask;

public class ProjectTaskCreateViewModel
{
    public Guid ProjectId { get; set; }
    public required string Name { get; set; }
    public Guid CreatedById { get; set; }
    public Guid? AssignedToId { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public string? Comment { get; set; }
    public int Priority { get; set; }
}