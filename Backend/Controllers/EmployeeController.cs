
using System.Security.Claims;
using Backend.Dtos.Common;
using Backend.Dtos.Employee;
using Backend.Interface;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    
    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService=employeeService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters query)
    {
        var employees=await _employeeService.GetAllAsync(query);
        return Ok(employees);
    }

    [Authorize]
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

    [Authorize(Roles ="Admin")]
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

    [Authorize(Roles ="Admin")]
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

    [Authorize(Roles ="Admin")]
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

    [Authorize(Roles ="Employee")]
    [HttpPost("complete-profile")]
    public async Task<IActionResult> CompleteProfile( [FromForm]CompleteEmployeeProfileDto dto)
    {
       var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User Id not founc");
        }
        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("Invalid usr Id");
        }

        var result=await _employeeService.CreateProfileAsync(Id,dto);

        if (!result)
        {
           return BadRequest("Unable to create employee profile"); 
        }

        return Ok(new
        {
            message="Profile completed successfully"
        });
    }

    [Authorize(Roles ="Employee")]
    [HttpGet("my-profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User not found");
        }

        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("Invalid user id");
        }

        var employeeProfile=await _employeeService.GetMyProfile(Id);

        if (employeeProfile == null)
        {
            return NotFound("Employee profile not found");
        }
        return Ok(employeeProfile);
    }

    [Authorize(Roles ="Employee")]
    [HttpPatch("update-profile")]
    public async Task<IActionResult> UpdateMyProfile([FromForm]UpdateEmployeeProfileDto dto)
    {
      var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User is not loggged");
        }

        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("User is not logged in");
        }

        var employee=await _employeeService.UpdateMyProfile(Id,dto);

        return Ok(employee);


    }
    [Authorize(Roles ="Employee")]
    [HttpPost("apply-leave")]
    public async Task<IActionResult> ApplyLeave(ApplyLeaveDto dto)
    {
        var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("User is not logged in");
        }
        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("User not logged in");
        }

        var leave=await _employeeService.ApplyLeave(Id,dto);
        return Ok(leave);
    }

}
