using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

//TODO: CancellationTokens
public class EmployeeRepository : IEmployeeRepository
{
    private readonly ProjectDbContext _context;

    public EmployeeRepository(ProjectDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _context.Employees.ToListAsync();

    public async Task<Employee?> GetByIdAsync(int id)
        => await _context.Employees.FindAsync(id);

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
    }

    public async Task<int> AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        
        return employee.Id;
    }

    public async Task<bool> UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var employeeToDelete = await _context.Employees.FindAsync(id);
        if (employeeToDelete != null)
        {
            _context.Employees.Remove(employeeToDelete);
            return await _context.SaveChangesAsync() > 0;
        }

        return false;
    }
}
