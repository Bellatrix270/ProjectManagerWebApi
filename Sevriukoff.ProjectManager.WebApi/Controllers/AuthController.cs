using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.WebApi.Controllers;

/// <summary>
/// Контроллер для аутентификации пользователей.
/// </summary>
[ApiController]
[Route("/api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Вход пользователя в систему.
    /// </summary>
    /// <param name="employee">Модель сотрудника, содержащая идентификатор и пароль.</param>
    /// <returns>Результат входа пользователя.</returns>
    /// <response code="200">Успешный вход пользователя.</response>
    /// <response code="401">Ошибка аутентификации. Неверные учетные данные.</response>
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody]EmployeeModel employee)
    {
        bool result;
        
        if (!string.IsNullOrEmpty(employee.Email))
            result = await _authService.LoginByEmailAsync(employee.Email, employee.Password);
        else if (!string.IsNullOrEmpty(employee.UserName))
            result = await _authService.LoginByUserNameAsync(employee.UserName, employee.Password);
        else
            result = await _authService.LoginByIdAsync(employee.Id!.Value, employee.Password);

        return result ? Ok() : BadRequest();
    }

    /// <summary>
    /// Выход пользователя из системы.
    /// </summary>
    /// <returns>Результат выхода пользователя.</returns>
    /// <response code="200">Успешный выход пользователя.</response>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        
        return Ok();
    }
}