using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Infrastructure.Dto;

namespace Sevriukoff.ProjectManager.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _employeeService.GetAll());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _employeeService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]EmployeeDto employeeDto)
    {
        try
        {
            var id = await _employeeService.AddAsync(employeeDto);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]EmployeeDto employeeDto)
    {
        try
        {
            employeeDto.Id = id;

            var success = await _employeeService.UpdateAsync(employeeDto);
            
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
        var success = await _employeeService.DeleteAsync(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}