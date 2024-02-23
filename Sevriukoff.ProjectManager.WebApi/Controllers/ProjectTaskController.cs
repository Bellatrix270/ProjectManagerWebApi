using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Models;

namespace Sevriukoff.ProjectManager.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ProjectTaskController : ControllerBase
{
    private readonly IProjectTaskService _projectTaskService;

    public ProjectTaskController(IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _projectTaskService.GetAllAsync());
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _projectTaskService.GetByIdAsync(id));
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