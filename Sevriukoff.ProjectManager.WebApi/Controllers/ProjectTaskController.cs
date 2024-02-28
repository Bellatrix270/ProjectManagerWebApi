using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.WebApi.Helpers;

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
    public async Task<IActionResult> GetAll([FromQuery] ProjectTaskQueryParameters queryParameters)
    {
        var userContext = UserContextHelper.GetUserContext(User);
    
        return Ok(await _projectTaskService.GetFilteredAndSortedAsync(
            queryParameters.Status, 
            queryParameters.Priority, 
            queryParameters.CreatedById, 
            queryParameters.AssignedToId,
            queryParameters.SortBy, 
            userContext, 
            queryParameters.Includes?.Split(';') ?? Array.Empty<string>()
        ));
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var task = await _projectTaskService.GetByIdAsync(id);
        
        return task == null ? NotFound() : Ok(task);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProjectTaskModel projectTaskModel)
    {
        try
        {
            var id = await _projectTaskService.AddAsync(projectTaskModel);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]ProjectTaskModel projectTaskModel)
    {
        try
        {
            var userContext = UserContextHelper.GetUserContext(User);
            
            projectTaskModel.Id = id;

            var success = await _projectTaskService.UpdateAsync(projectTaskModel);
            
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _projectTaskService.DeleteAsync(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}