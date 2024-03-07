using Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;
#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.Project;

public class ProjectViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string CustomerCompany { get; set; }
    public required string ExecutorCompany { get; set; }
    public DateTime StartDate  { get; set; }
    public DateTime EndDate  { get; set; }
    public int Priority { get; set; }

    public Guid ManagerId { get; set; }
    public List<EmployeeViewModel>? Employees { get; set; }
    public List<ProjectViewModel>? Tasks { get; set; }
}