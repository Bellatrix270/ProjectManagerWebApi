using Microsoft.AspNetCore.Identity;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthManager _authManager;

    public AuthService(IAuthManager authManager)
    {
        _authManager = authManager;
    }
    
    public async Task<(Guid Id, IEnumerable<IdentityError> Errors)> RegisterAsync(EmployeeModel employee, string password)
    {
        var validationResult = employee.IsValid();

        if (!validationResult.IsValid)
            return (Guid.Empty, validationResult.Errors.Select(MapperWrapper.Map<IdentityError>));
        
        return await _authManager.RegisterAsync(MapperWrapper.Map<Employee>(employee), password);
    }

    public async Task<bool> LoginByIdAsync(Guid employeeId, string password)
    {
        return (await _authManager.LoginByIdAsync(employeeId, password)).Succeeded;
    }
    
    public async Task<bool> LoginByEmailAsync(string email, string password)
    {
        return (await _authManager.LoginByEmailAsync(email, password)).Succeeded;
    }

    public async Task<bool> LoginByUserNameAsync(string userName, string password)
    {
        return (await _authManager.LoginByUserNameAsync(userName, password)).Succeeded;
    }

    public async Task LogoutAsync()
    {
        await _authManager.LogoutAsync();
    }
}