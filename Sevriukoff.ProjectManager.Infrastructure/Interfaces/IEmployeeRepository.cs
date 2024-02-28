using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Infrastructure.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(Guid id);
    Task<Employee?> GetByEmailAsync(string email);
    Task<bool> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(Guid id);
}