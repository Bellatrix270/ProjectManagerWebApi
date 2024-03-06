using Microsoft.AspNetCore.Identity;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Authorization;

public class Employee: IdentityUser<Guid>, IBaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    
    public string Role { get; set; }
    public bool IsFirstLogin { get; set; } = true;
}
