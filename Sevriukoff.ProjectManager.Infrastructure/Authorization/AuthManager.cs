using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Authorization;

public class AuthManager : IAuthManager
{
    private readonly UserManager<Employee> _userManager;
    private readonly SignInManager<Employee> _signInManager;

    public AuthManager(UserManager<Employee> userManager, SignInManager<Employee> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<(Guid Id, IEnumerable<IdentityError> Errors)> RegisterAsync(Employee employee, string password)
    {
        var result = await _userManager.CreateAsync(employee, password);

        if (result.Succeeded)
           await _userManager.AddClaimAsync(employee, new Claim(ClaimTypes.Role, employee.Role));

        return (employee.Id, result.Errors);
    }

    public async Task<SignInResult> LoginByIdAsync(Guid userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        return result;
    }
    
    public async Task<SignInResult> LoginByEmailAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        return result;
    }
    
    public async Task<SignInResult> LoginByUserNameAsync(string nickName, string password)
    {
        var user = await _userManager.FindByNameAsync(nickName);
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> ChangePasswordAsync()
    {
        throw new NotImplementedException();
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}