using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Entities.TypeConfigurations;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

//TODO: CancellationTokens
public class EmployeeRepository : IEmployeeRepository
{
    private readonly ProjectDbContext _projectContext;
    private readonly AuthDbContext _authContext;

    public EmployeeRepository(ProjectDbContext projectContext, AuthDbContext authContext)
    {
        _projectContext = projectContext;
        _authContext = authContext;
    }
    
    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _authContext.Users.ToListAsync();

    public async Task<Employee?> GetByIdAsync(Guid id)
        => await _authContext.Users.FindAsync(id);

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _authContext.Users.FirstOrDefaultAsync(emp => emp.Email == email);
    }
    
    public async Task<bool> UpdateAsync(Employee employee)
    {
        _authContext.Users.Update(employee);
        return await _authContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var employeeToDelete = await _authContext.Users.FindAsync(id);
        if (employeeToDelete != null)
        {
            _authContext.Users.Remove(employeeToDelete);
            return await _authContext.SaveChangesAsync() > 0;
        }

        return false;
    }
}
