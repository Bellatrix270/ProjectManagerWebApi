using Sevriukoff.ProjectManager.Infrastructure.Dto;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAll();
    Task<EmployeeDto> GetById(int id);
    Task<int> AddAsync(EmployeeDto employee);
    Task<bool> UpdateAsync(EmployeeDto employee);
    Task<bool> DeleteAsync(int id);
}
