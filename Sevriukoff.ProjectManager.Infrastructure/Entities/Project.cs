using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities.TypeConfigurations;

namespace Sevriukoff.ProjectManager.Infrastructure.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; }
    public string CustomerCompany { get; set; }
    public string ExecutorCompany { get; set; }
    public DateTime StartDate  { get; set; }
    public DateTime EndDate  { get; set; }
    public int Priority { get; set; }

    public Guid ManagerId { get; set; }
    public List<Guid> Employees { get; set; }
    public List<ProjectTask> Tasks { get; set; }
}