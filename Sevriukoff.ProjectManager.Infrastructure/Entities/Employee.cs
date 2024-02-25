using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Infrastructure.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }

    public List<Project> Projects { get; set; }
}