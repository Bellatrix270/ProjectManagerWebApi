namespace Sevriukoff.ProjectManager.Infrastructure.Dto;

public class EmployeeDto
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; }
}
