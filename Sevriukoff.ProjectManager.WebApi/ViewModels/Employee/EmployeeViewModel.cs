using Sevriukoff.ProjectManager.Application.Models;
#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;

public class EmployeeViewModel
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public UserRole Role { get; set; }
}