using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.WebApi.Controllers;

/// <summary>
/// Контроллер для управления сотрудниками
/// </summary>
[ApiController]
[Authorize(Policy = nameof(UserRole.Administrator))]
[Route("/api/v1/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;

    public EmployeeController(IEmployeeService employeeService, IAuthService authService)
    {
        _employeeService = employeeService;
        _authService = authService;
    }
    
    /// <summary>
    /// Получает список всех сотрудников.
    /// </summary>
    /// <returns>Список всех сотрудников.</returns>
    /// <response code="200">Список сотрудников успешно получен.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    [ProducesResponseType(typeof(IEnumerable<EmployeeModel>), 200)]
    [ProducesResponseType(401)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _employeeService.GetAllAsync());
    }
    
    /// <summary>
    /// Получает информацию о сотруднике по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>Информация о сотруднике.</returns>
    /// <response code="200">Информация о сотруднике успешно получена.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Сотрудник с указанным идентификатором не найден.</response>
    [ProducesResponseType(typeof(EmployeeModel), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        
        return employee == null ? NotFound() : Ok(employee);
    }
    
    /// <summary>
    /// Регистрирует нового сотрудника.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/employee
    ///     {
    ///         "id": "a62b3a2f-93be-4db2-ac46-9045a320d44b",
    ///         "UserName": "IgorSeff",
    ///         "FirstName": "Igor",
    ///         "LastName": "Sevriukoff",
    ///         "Patronymic": "Vadimovich",
    ///         "Email": "igor.seff@example.com",
    ///         "Password": "password123",
    ///         "Role": 0
    ///     }
    /// 
    /// </remarks>
    /// <param name="employeeModel">Модель нового сотрудника.</param>
    /// <returns>Результат регистрации нового сотрудника.</returns>
    /// <response code="201">Сотрудник успешно зарегистрирован.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]EmployeeModel employeeModel)
    {
        try
        {
            var (id, errors) = await _authService.RegisterAsync(employeeModel);

            if (!errors.Any())
                return CreatedAtAction(nameof(Get), new { id }, id);

            return BadRequest(errors);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    /// <summary>
    /// Обновляет информацию о сотруднике по его идентификатору.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/employee/a62b3a2f-93be-4db2-ac46-9045a320d44b
    ///     {
    ///         "Email": "igor.seff270@example.com",
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <param name="employeeModel">Модель обновленной информации о сотруднике.</param>
    /// <returns>Результат обновления информации о сотруднике.</returns>
    /// <response code="204">Информация о сотруднике успешно обновлена.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Сотрудник с указанным идентификатором не найден.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody]EmployeeModel employeeModel)
    {
        try
        {
            employeeModel.Id = id;

            var success = await _employeeService.UpdateAsync(employeeModel);
            
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет сотрудника по его идентификатору.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     DELETE /api/v1/employee/a62b3a2f-93be-4db2-ac46-9045a320d44b
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>Результат удаления сотрудника.</returns>
    /// <response code="204">Сотрудник успешно удален.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Сотрудник с указанным идентификатором не найден.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _employeeService.DeleteAsync(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}