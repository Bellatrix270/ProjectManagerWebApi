using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;
#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.ProjectTask;

public class ProjectTaskViewModel
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public required string Name { get; set; }
    
    public Guid CreatedById { get; set; }
    public EmployeeViewModel? CreatedBy { get; set; }
    
    public Guid? AssignedToId { get; set; }
    public EmployeeViewModel? AssignedTo { get; set; }
    
    public ProjectTaskStatus Status { get; set; }
    public string? Comment { get; set; }
    public int Priority { get; set; }
}