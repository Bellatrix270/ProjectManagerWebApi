namespace Sevriukoff.ProjectManager.Infrastructure.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }

    public List<Project> Projects { get; set; }
}