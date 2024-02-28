using Microsoft.AspNetCore.Identity;

namespace Sevriukoff.ProjectManager.Infrastructure.Authorization;

public class Employee: IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    
    public string Role { get; set; }
    public bool IsFirstLogin { get; set; } = true;
}