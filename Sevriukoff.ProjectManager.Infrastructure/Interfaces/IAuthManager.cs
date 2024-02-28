using Microsoft.AspNetCore.Identity;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;

namespace Sevriukoff.ProjectManager.Infrastructure.Interfaces;

public interface IAuthManager
{
    Task<(Guid Id, IEnumerable<IdentityError> Errors)> RegisterAsync(Employee employee, string password);
    Task<SignInResult> LoginByIdAsync(Guid userId, string password);
    Task<SignInResult> LoginByEmailAsync(string email, string password);
    Task<SignInResult> LoginByUserNameAsync(string userName, string password);
    Task LogoutAsync();
}