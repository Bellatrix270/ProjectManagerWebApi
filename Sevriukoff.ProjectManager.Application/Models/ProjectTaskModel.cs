using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Models;

public class ProjectTaskModel
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public int CreatedById { get; set; }
    public int? AssignedToId { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public string Comment { get; set; }
    public int Priority { get; set; }
}