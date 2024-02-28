namespace Sevriukoff.ProjectManager.Application.Models;

public class EmployeeModel
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public UserRole? Role { get; set; }
}

public enum UserRole
{
    Employee,
    Manager,
    Administrator
}

