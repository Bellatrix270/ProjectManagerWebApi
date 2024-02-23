using Microsoft.AspNetCore.Mvc;
using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Models;

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
        return Ok(await _employeeService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _employeeService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]EmployeeModel employeeModel)
    {
        try
        {
            var id = await _employeeService.AddAsync(employeeModel);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errorMessage = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody]EmployeeModel employeeModel)
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _employeeService.DeleteAsync(id);
        
        if (!success)
            return NotFound();

        return NoContent();
    }
}