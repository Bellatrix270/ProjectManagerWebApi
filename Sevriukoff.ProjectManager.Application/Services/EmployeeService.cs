using System.Text.RegularExpressions;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Infrastructure.Dto;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Repository;

namespace Sevriukoff.ProjectManager.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAll()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto> GetById(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            
        }

        return MapToDto(employee);
    }

    public async Task<int> AddAsync(EmployeeDto employeeDto)
    {
        if (!IsValidEmployee(employeeDto, out string errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }
        
        var employee = MapFromDto(employeeDto); 
        var id = await _employeeRepository.AddAsync(employee);

        return id;
    }

    public async Task<bool> UpdateAsync(EmployeeDto employeeDto)
    {
        if (!IsValidEmployee(employeeDto, out string errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }
        
        var employee = MapFromDto(employeeDto);
        return await _employeeRepository.UpdateAsync(employee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _employeeRepository.DeleteAsync(id);
    }

    #region Mapping

    private EmployeeDto MapToDto(Employee emp)
    {
        return new EmployeeDto
        {
            Id = emp.Id,
            FirstName = emp.Firstname,
            LastName = emp.Lastname,
            Patronymic = emp.Patronymic,
            FullName = string.Join(' ', emp.Lastname, emp.Firstname, emp.Patronymic),
            Email = emp.Email
        };
    }

    private Employee MapFromDto(EmployeeDto emp)
    {
        //var fullName = emp.FullName.Split(' ');
        
        return new Employee
        {
            Id = emp.Id ?? 0,
            Firstname = emp.FirstName,
            Lastname = emp.LastName,
            Patronymic = emp.Patronymic,
            Email = emp.Email
        };
    }

    #endregion

    #region Validate

    private bool IsValidEmployee(EmployeeDto employeeDto, out string errorMessage)
    {
        errorMessage = string.Empty;
        
        var isValidName = !employeeDto.FirstName.Any(char.IsDigit); //TODO: Validate

        if (!isValidName)
        {
            errorMessage = "Invalid name.";
            return false;
        }

        if (!IsValidEmail(employeeDto.Email))
        {
            errorMessage = "Invalid email format.";
            return false;
        }

        if (!IsUniqueEmail(employeeDto.Email))
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