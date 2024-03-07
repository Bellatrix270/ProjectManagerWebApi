using Sevriukoff.ProjectManager.Application.Models;
#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;

public class EmployeeCreateViewModel
{
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Patronymic { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; }
}