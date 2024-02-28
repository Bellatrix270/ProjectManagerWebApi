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

    public async Task<bool> UpdateAsync(EmployeeModel employeeModel)
    {
        if (!IsValidEmployee(employeeModel, out string errorMessage))
        {
            throw new ValidationException(errorMessage);
        }
        
        var employee = MapperWrapper.Map<Employee>(employeeModel);
        return await _employeeRepository.UpdateAsync(employee);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _employeeRepository.DeleteAsync(id);
    }

    #region Validate

    private bool IsValidEmployee(EmployeeModel employeeModel, out string errorMessage)
    {
        errorMessage = string.Empty;
        
        var isValidName = !employeeModel.FirstName.Any(char.IsDigit); //TODO: Validate

        if (!isValidName)
        {
            errorMessage = "Invalid name.";
            return false;
        }

        if (!IsValidEmail(employeeModel.Email))
        {
            errorMessage = "Invalid email format.";
            return false;
        }

        if (!IsUniqueEmail(employeeModel.Email))
        {
            errorMessage = "Employee with this email already exists.";
            return false;
        }
        
        return true;
    }

    private bool IsValidEmail(string email)
    {
        const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        
        return Regex.IsMatch(email, pattern);
    }

    private bool IsUniqueEmail(string email)
    {
        var existingEmployee = _employeeRepository.GetByEmailAsync(email).GetAwaiter().GetResult();

        return existingEmployee == null;
    }

    #endregion
}