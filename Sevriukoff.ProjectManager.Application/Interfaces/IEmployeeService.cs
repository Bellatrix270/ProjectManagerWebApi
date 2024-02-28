using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> GetAllAsync();
    Task<EmployeeModel?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(EmployeeModel employee);
    Task<bool> DeleteAsync(Guid id);
}
