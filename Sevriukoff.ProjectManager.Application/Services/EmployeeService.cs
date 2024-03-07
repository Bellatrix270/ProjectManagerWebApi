using System.Text.RegularExpressions;
using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeModel>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return employees.Select(MapperWrapper.Map<EmployeeModel>);
    }

    public async Task<EmployeeModel?> GetByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        return MapperWrapper.Map<Employee, EmployeeModel>(employee);
    }

    public async Task<(bool success, IEnumerable<ValidationError> Errors)> UpdateAsync(EmployeeModel employeeModel)
    {
        var (isValid, errors) = employeeModel.IsValid();
        
        if (!isValid)
            return (isValid, errors);
        
        var employee = MapperWrapper.Map<Employee>(employeeModel);
        return (await _employeeRepository.UpdateAsync(employee), errors);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _employeeRepository.DeleteAsync(id);
    }
}