using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _projectService.GetByIdAsync(id));
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
    /// <param name="startDateFrom">Дата начала периода, в котором был запущен проект.</param>
    /// <param name="startDateTo">Дата окончания периода, в котором был запущен проект.</param>
    /// <param name="priority">Приоритет проекта от 1 до 10.</param>
    /// <param name="includes">Список связанных сущностей для включения их данных в ответ (разделенный точкой с запятой список имен связанных сущностей).</param>
    /// <param name="sortBy">Поле, по которому происходит сортировка. Поддерживаются поля основной сущности проекта. Например sortBy=PriorityDesc</param>
    /// <returns>Список проектов, отфильтрованный и отсортированный в соответствии с переданными параметрами.</returns>
    /// <response code="200">Вовзращяет результирующий список проектов</response>
    /// <response code="400">Некоректная настройка фильтров. Подробности в errorMessage.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? startDateFrom,
        [FromQuery] DateTime? startDateTo,
        [FromQuery] int? priority,
        [FromQuery] string? includes,
        [FromQuery] string? sortBy)
    {
        try
        {
            var queryCount = HttpContext.Request.Query.Count;
        
            if (queryCount == 1 && !string.IsNullOrEmpty(includes) || queryCount == 0)
                return Ok(await _projectService.GetAllAsync(includes?.Split(';') ?? Array.Empty<string>()));
        
            return Ok(await _projectService.GetFilteredAndSortedAsync(startDateFrom, startDateTo, priority, sortBy));
        }
        catch (SpecificationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProjectModel projectModel)
    {
        try
        {
            var id = await _projectService.AddAsync(projectModel);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]ProjectModel projectModel)
    {
        try
        {
            projectModel.Id = id;

            var success = await _projectService.UpdateAsync(projectModel);
            
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _projectService.DeleteAsync(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
    
    [HttpPost("{projectId:int}/employees/{employeeId:int}")]
    public async Task<IActionResult> AddEmployeeToProject(int projectId, int employeeId)
    {
        try
        {
            var success = await _projectService.AddEmployeeToProjectAsync(projectId, employeeId);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    [HttpDelete("{projectId:int}/employees/{employeeId:int}")]
    public async Task<IActionResult> RemoveEmployeeFromProject(int projectId, int employeeId)
    {
        try
        {
            var success = await _projectService.RemoveEmployeeFromProjectAsync(projectId, employeeId);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
    
    [HttpPost("{projectId:int}/tasks/{projectTaskId:int}")]
    public async Task<IActionResult> AddTaskToProject(int projectId, int projectTaskId)
    {
        try
        {
            var success = await _projectService.AddTaskToProjectAsync(projectId, projectId);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    [HttpDelete("{projectId:int}/tasks/{projectTaskId:int}")]
    public async Task<IActionResult> Delete(int projectId, int projectTaskId)
    {
        try
        {
            var success = await _projectService.RemoveTaskFromProjectAsync(projectId, projectTaskId);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }
}