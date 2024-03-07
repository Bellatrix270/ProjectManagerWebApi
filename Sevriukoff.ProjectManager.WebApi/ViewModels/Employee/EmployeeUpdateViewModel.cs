using Sevriukoff.ProjectManager.Application.Models;

#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;

public class EmployeeUpdateViewModel
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}