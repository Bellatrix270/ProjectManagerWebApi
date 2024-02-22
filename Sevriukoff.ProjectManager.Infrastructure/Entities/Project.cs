namespace Sevriukoff.ProjectManager.Infrastructure.Entities;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CustomerCompany { get; set; }
    public string ExecutorCompany { get; set; }
    public DateTime StartDate  { get; set; }
    public DateTime EndDate  { get; set; }
    public int Priority { get; set; }

    public int ManagerId { get; set; }
    public Employee Manager { get; set; }
    public List<Employee> Employees { get; set; }
}