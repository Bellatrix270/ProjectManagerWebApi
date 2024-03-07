using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.WebApi.Helpers;
using Sevriukoff.ProjectManager.WebApi.ViewModels.Project;

namespace Sevriukoff.ProjectManager.WebApi.Controllers;

/// <summary>
/// Контроллер для управления проектами
/// </summary>
[ApiController]
[Authorize(Policy = nameof(UserRole.Manager))]
[Route("/api/v1/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Получает информацию о проекте по его идентификатору.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/project/1
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Информация о проекте.</returns>
    /// <response code="200">Информация о проекте успешно получена.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Проект с указанным идентификатором не найден.</response>
    [ProducesResponseType(typeof(EmployeeModel), 200)]
    [ProducesResponseType(404)]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectViewModel>> Get(Guid id)
    {
        var project = MapperWrapper.Map<ProjectModel, ProjectViewModel>(await _projectService.GetByIdAsync(id));
        
        return project == null ? NotFound() : Ok(project);
    }

    /// <summary>
    /// Получает список проектов с возможностью фильтрации, сортировки и настройки связанных свойств.
    /// </summary>
    /// <remarks>
    /// Примеры запросов:
    /// - /project?startDateFrom=2023-01-01&amp;startDateTo=2025-01-01&amp;priority=3&amp;sortBy=PriorityDesc&amp;includes=Employees
    /// - /project?startDateFrom=2023-01-01&amp;startDateTo=2025-01-01&amp;priority=3&amp;sortBy=Name&amp;includes=Tasks;Employees
    /// - /project
    /// </remarks>
    /// <param name="queryParameters">Параметры запроса.</param>
    /// <returns>Список проектов, отфильтрованный и отсортированный в соответствии с переданными параметрами.</returns>
    /// <response code="200">Вовзращяет результирующий список проектов</response>
    /// <response code="400">Некоректная настройка фильтров. Подробности в errorMessage.</response>
    /// <response code="401">Доступ запрещён.</response>
    [ProducesResponseType(typeof(IEnumerable<ProjectModel>), 200)]
    [ProducesResponseType(400)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectViewModel>>> GetAll([FromQuery] ProjectQueryParameters queryParameters)
    {
        try
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var queryCount = HttpContext.Request.Query.Count;
            var userContext = UserContextHelper.GetUserContext(User);

            IEnumerable<ProjectModel> projectsModel;

            if ((userRole == nameof(UserRole.Administrator) && queryCount == 1 &&
                 !string.IsNullOrEmpty(queryParameters.Includes)) ||
                (userRole == nameof(UserRole.Administrator) && queryCount == 0))
            {
                projectsModel = await _projectService.GetAllAsync
                (
                    queryParameters.Includes?.Split(';') ?? Array.Empty<string>()
                );
            }
            else
            {
                projectsModel = await _projectService.GetFilteredAndSortedAsync
                (
                    queryParameters.StartDateFrom,
                    queryParameters.StartDateTo,
                    queryParameters.Priority,
                    queryParameters.SortBy,
                    userContext,
                    queryParameters.Includes?.Split(';') ?? Array.Empty<string>()
                );
            }

            return Ok(projectsModel.Select(MapperWrapper.Map<ProjectViewModel>));
        }
        catch (SpecificationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
    /// <summary>
    /// Создает новый проект.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/project
    ///     {
    ///         "Name" : "Новый проект",
    ///         "CustomerCompany": "Название заказчика",
    ///         "ExecutorCompany": "Название исполнителя",
    ///         "StartDate": "2024-02-20",
    ///         "EndDate": "2024-02-25",
    ///         "Priority": 3,
    ///         "ManagerId": "4fc2bbd4-9e5e-4286-b943-600d9c82fecc"
    ///     }
    /// 
    /// </remarks>
    /// <param name="projectViewModel">Модель нового проекта.</param>
    /// <returns>Результат создания нового проекта.</returns>
    /// <response code="201">Проект успешно создан.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещён.</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProjectCreateViewModel projectViewModel)
    {
        try
        {
            var projectModel = MapperWrapper.Map<ProjectModel>(projectViewModel);
            
            var (id, errors) = await _projectService.AddAsync(projectModel);

            if (!errors.Any())
                return CreatedAtAction(nameof(Get), new { id }, id);

            return BadRequest(errors);
        }
        catch (Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
    
    /// <summary>
    /// Обновляет информацию о проекте по его идентификатору.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     PUT /api/v1/project/1
    ///     {
    ///         "Name": "Измененное название проекта",
    ///         "Priority": 5
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор проекта.</param>
    /// <param name="projectViewModel">Модель обновленной информации о проекте.</param>
    /// <returns>Результат обновления информации о проекте.</returns>
    /// <response code="204">Информация о проекте успешно обновлена.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещён.</response>
    /// <response code="404">Проект с указанным идентификатором не найден.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody]ProjectUpdateViewModel projectViewModel)
    {
        try
        {
            var userContext = UserContextHelper.GetUserContext(User);
            
            projectViewModel.Id = id;
            var projectModel = MapperWrapper.Map<ProjectModel>(projectViewModel);

            var (success, errors) = await _projectService.UpdateAsync(projectModel, userContext);

            return success switch
            {
                true => NoContent(),
                false when !errors.Any() => NotFound(),
                _ => BadRequest(errors)
            };
        }
        catch (Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
    
    /// <summary>
    /// Удаляет проект по его идентификатору.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     DELETE /api/v1/project/1
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Результат удаления проекта.</returns>
    /// <response code="204">Проект успешно удален.</response>
    /// <response code="401">Доступ запрещён.</response>
    /// <response code="404">Проект с указанным идентификатором не найден.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _projectService.DeleteAsync(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
    
    /// <summary>
    /// Добавляет сотрудника к проекту.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/project/1/employees/a62b3a2f-93be-4db2-ac46-9045a320d44b
    /// 
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <param name="employeeId">Идентификатор сотрудника.</param>
    /// <returns>Результат добавления сотрудника к проекту.</returns>
    /// <response code="204">Сотрудник успешно добавлен к проекту.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или не достаточный уровень прав.</response>
    /// <response code="404">Проект или сотрудник с указанным идентификатором не найден.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [HttpPost("{projectId:guid}/employees/{employeeId:guid}")]
    public async Task<IActionResult> AddEmployeeToProject(Guid projectId, Guid employeeId)
    {
        try
        {
            var userContext = UserContextHelper.GetUserContext(User);
            
            var success = await _projectService.AddEmployeeToProjectAsync(projectId, employeeId, userContext);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (AccessDeniedException ex)
        {
            return Unauthorized(new { errorMessage = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет сотрудника из проекта.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     DELETE /api/v1/project/1/employees/a62b3a2f-93be-4db2-ac46-9045a320d44b
    /// 
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <param name="employeeId">Идентификатор сотрудника.</param>
    /// <returns>Результат удаления сотрудника из проекта.</returns>
    /// <response code="204">Сотрудник успешно удален из проекта.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Проект или сотрудник с указанным идентификатором не найден.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpDelete("{projectId:guid}/employees/{employeeId:guid}")]
    public async Task<IActionResult> RemoveEmployeeFromProject(Guid projectId, Guid employeeId)
    {
        try
        {
            var userContext = UserContextHelper.GetUserContext(User);
            
            var success = await _projectService.RemoveEmployeeFromProjectAsync(projectId, employeeId, userContext);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
    
    /// <summary>
    /// Добавляет задачу к проекту.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/project/1/tasks/2
    /// 
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <param name="projectTaskId">Идентификатор задачи.</param>
    /// <returns>Результат добавления задачи к проекту.</returns>
    /// <response code="204">Задача успешно добавлена к проекту.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Проект или задача с указанным идентификатором не найдены.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpPost("{projectId:guid}/tasks/{projectTaskId:guid}")]
    public async Task<IActionResult> AddTaskToProject(Guid projectId, Guid projectTaskId)
    {
        try
        {
            var userContext = UserContextHelper.GetUserContext(User);
            
            var success = await _projectService.AddTaskToProjectAsync(projectId, projectTaskId, userContext);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    /// <summary>
    /// Удаляет задачу из проекта.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     DELETE /api/v1/project/1/tasks/2
    /// 
    /// </remarks>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <param name="projectTaskId">Идентификатор задачи.</param>
    /// <returns>Результат удаления задачи из проекта.</returns>
    /// <response code="204">Задача успешно удалена из проекта.</response>
    /// <response code="400">Ошибка в запросе или неверные данные. Подробности в сообщении об ошибке.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Проект или задача с указанным идентификатором не найдены.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpDelete("{projectId:guid}/tasks/{projectTaskId:guid}")]
    public async Task<IActionResult> RemoveTaskFromProject(Guid projectId, Guid projectTaskId)
    {
        try
        {
            var userContext = UserContextHelper.GetUserContext(User);
            
            var success = await _projectService.RemoveTaskFromProjectAsync(projectId, projectTaskId, userContext);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
}