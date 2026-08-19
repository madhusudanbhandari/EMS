
using Backend.Dtos.Employee;
using Backend.Interface;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;


[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService=employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees=await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee=await _employeeService.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound("Employee not found");
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        var employee=await _employeeService.CreateAsync(dto);

        if (employee == null)
        {
            return BadRequest("Department does not exist");
        }

        return CreatedAtAction(
            nameof(GetById),
            new{id=employee.Id},
            employee
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,UpdateEmployeeDto dto)
    {
        var updated=await _employeeService.UpdateAsync(id,dto);
        if (!updated)
        {
            return BadRequest("Employee or Department not found");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted=await _employeeService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound("Employee not found");
        }

        return NoContent();
    }



}
