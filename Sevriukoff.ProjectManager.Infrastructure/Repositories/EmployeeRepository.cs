using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities.TypeConfigurations;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

//TODO: CancellationTokens
public class EmployeeRepository : BaseRepository<Employee, Guid, AuthDbContext>, IEmployeeRepository
{
    public EmployeeRepository(AuthDbContext context) : base(context) { }
    
    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(emp => emp.Email == email);
    }
}
