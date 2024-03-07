using Microsoft.AspNetCore.Identity;
using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.Application.Interfaces;

public interface IAuthService
{
    Task<(Guid Id, IEnumerable<IdentityError> Errors)> RegisterAsync(EmployeeModel employee, string password);
    Task<bool> LoginByIdAsync(Guid employeeId, string password);
    Task<bool> LoginByEmailAsync(string email, string password);
    Task<bool> LoginByUserNameAsync(string userName, string password);
    Task LogoutAsync();
}