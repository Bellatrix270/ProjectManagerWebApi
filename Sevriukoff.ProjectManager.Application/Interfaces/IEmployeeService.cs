using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> GetAllAsync();
    Task<EmployeeModel> GetByIdAsync(int id);
    Task<int> AddAsync(EmployeeModel employee);
    Task<bool> UpdateAsync(EmployeeModel employee);
    Task<bool> DeleteAsync(int id);
}
