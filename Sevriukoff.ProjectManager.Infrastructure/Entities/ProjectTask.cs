namespace Sevriukoff.ProjectManager.Infrastructure.Entities;

public class ProjectTask
{
    public int Id { get; set; }
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

public enum ProjectTaskStatus
{
    ToDo,
    InProgress,
    Done
}