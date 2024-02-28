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
        var project = await _projectService.GetByIdAsync(id);
        
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
    public async Task<IActionResult> GetAll([FromQuery] ProjectQueryParameters queryParameters)
    {
        try
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var queryCount = HttpContext.Request.Query.Count;
            var userContext = UserContextHelper.GetUserContext(User);

            if ((userRole == nameof(UserRole.Administrator) && queryCount == 1 && !string.IsNullOrEmpty(queryParameters.Includes)) ||
                (userRole == nameof(UserRole.Administrator) && queryCount == 0))
                return Ok(await _projectService.GetAllAsync(queryParameters.Includes?.Split(';') ?? Array.Empty<string>()));

            return Ok(await _projectService.GetFilteredAndSortedAsync(queryParameters.StartDateFrom, queryParameters.StartDateTo, queryParameters.Priority, queryParameters.SortBy, userContext, queryParameters.Includes?.Split(';') ?? Array.Empty<string>()));

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