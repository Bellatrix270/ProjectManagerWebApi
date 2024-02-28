using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sevriukoff.ProjectManager.Infrastructure.Authorization;

public class AuthDbContext : IdentityDbContext<Employee, Role, Guid>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
}