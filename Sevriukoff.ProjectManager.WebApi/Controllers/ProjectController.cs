using System.Linq.Expressions;
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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? startDateFrom, [FromQuery] DateTime? startDateTo, [FromQuery] int? priority)
    {
        if (startDateFrom == null && startDateTo == null && priority == null)
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }

        var filteredProjects = _projectService.GetFiltered(startDateFrom.Value, startDateTo.Value, priority.Value);
        return Ok(filteredProjects);
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