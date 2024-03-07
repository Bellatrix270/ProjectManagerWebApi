using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Mapping;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.WebApi.Helpers;
using Sevriukoff.ProjectManager.WebApi.ViewModels.ProjectTask;

namespace Sevriukoff.ProjectManager.WebApi.Controllers;

/// <summary>
/// Контроллер для управления задачами
/// </summary>
[ApiController]
[Authorize(Policy = nameof(UserRole.Employee))]
[Route("/api/v1/[controller]")]
public class ProjectTaskController : ControllerBase
{
    private readonly IProjectTaskService _projectTaskService;

    public ProjectTaskController(IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }
    
    /// <summary>
    /// Получает список задач проекта с возможностью фильтрации, сортировки и настройки связанных свойств.
    /// </summary>
    /// <remarks>
    /// Примеры запросов:
    /// 
    ///     GET /api/v1/projecttask
    ///     GET /api/v1/projecttask
    ///     GET /api/v1/projecttask
    /// 
    /// </remarks>
    /// <param name="queryParameters">Параметры запроса.</param>
    /// <returns>Список задач проекта, отфильтрованный и отсортированный в соответствии с переданными параметрами.</returns>
    /// <response code="200">Возвращает результирующий список задач проекта.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    [ProducesResponseType(typeof(IEnumerable<ProjectTaskModel>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectTaskViewModel>>> GetAll([FromQuery]ProjectTaskQueryParameters queryParameters)
    {
        var userContext = UserContextHelper.GetUserContext(User);
        var projectTasksModel = await _projectTaskService.GetFilteredAndSortedAsync(
            queryParameters.Status,
            queryParameters.Priority,
            queryParameters.CreatedById,
            queryParameters.AssignedToId,
            queryParameters.SortBy,
            userContext,
            queryParameters.Includes?.Split(';') ?? Array.Empty<string>()
        );
    
        return Ok(projectTasksModel.Select(MapperWrapper.Map<ProjectTaskViewModel>));
    }
    
    /// <summary>
    /// Получает задачу проекта по её идентификатору.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/projecttask/1
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор задачи проекта.</param>
    /// <returns>Задача проекта с указанным идентификатором.</returns>
    /// <response code="200">Возвращает задачу проекта с указанным идентификатором.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Задача с указанным идентификатором не найдена.</response>
    [ProducesResponseType(typeof(ProjectTaskModel), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectTaskViewModel>> Get(Guid id)
    {
        var task = MapperWrapper.Map<ProjectTaskModel, ProjectTaskViewModel>(await _projectTaskService.GetByIdAsync(id));
        
        return task == null ? NotFound() : Ok(task);
    }
    
    /// <summary>
    /// Создает новую задачу проекта.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     POST /api/v1/projecttask
    ///     {
    ///         "ProjectId": 1,
    ///         "Name": "Название задачи",
    ///         "CreatedById": "2991E2CA-E64A-4832-8C5D-B2F8B9A1F305",
    ///         "Status": 0,
    ///         "Comment": "Описание задачи",
    ///         "Priority": 1
    ///     }
    /// 
    /// </remarks>
    /// <param name="projectTaskViewModel">Модель задачи проекта для создания.</param>
    /// <returns>Созданную задачу проекта.</returns>
    /// <response code="201">Возвращает созданную задачу проекта.</response>
    /// <response code="400">Возвращается в случае, если переданы некорректные данные для создания задачи.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [Authorize(Policy = nameof(UserRole.Manager))]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProjectTaskCreateViewModel projectTaskViewModel)
    {
        try
        {
            var projectTaskModel = MapperWrapper.Map<ProjectTaskModel>(projectTaskViewModel);
            
            var (id, errors) = await _projectTaskService.AddAsync(projectTaskModel);

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
    /// Обновляет существующую задачу проекта.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     PUT /api/v1/projecttask/1
    ///     {
    ///         "Status": 2,
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор задачи проекта для обновления.</param>
    /// <param name="projectTaskViewModel">Модель задачи проекта с обновленными данными.</param>
    /// <returns>Без содержимого.</returns>
    /// <response code="204">Возвращает успешный результат обновления задачи проекта.</response>
    /// <response code="400">Возвращается в случае, если переданы некорректные данные для обновления задачи.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Задача проекта с указанным идентификатором не найдена.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody]ProjectTaskUpdateViewModel projectTaskViewModel)
    {
        try
        {
            var userContext = UserContextHelper.GetUserContext(User);
            
            projectTaskViewModel.Id = id;
            var projectTaskModel = MapperWrapper.Map<ProjectTaskModel>(projectTaskViewModel);

            var (success, errors) = await _projectTaskService.UpdateAsync(projectTaskModel, userContext);
            
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
    /// Удаляет задачу по её идентификатору.
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     DELETE /api/v1/projecttask/1
    /// 
    /// </remarks>
    /// <param name="id">Идентификатор задачи проекта для удаления.</param>
    /// <returns>Без содержимого.</returns>
    /// <response code="204">Возвращает успешный результат удаления задачи проекта.</response>
    /// <response code="401">Доступ запрещен. Пользователь не авторизован или имеет не достаточный уровень прав.</response>
    /// <response code="404">Задача проекта с указанным идентификатором не найдена.</response>
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [Authorize(Policy = nameof(UserRole.Administrator))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _projectTaskService.DeleteAsync(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}