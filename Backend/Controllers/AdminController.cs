
using Backend.Dtos.Admin;
using Backend.Interface;
using Backend.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[Authorize(Roles ="Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService=adminService;
    }

    [HttpGet("pending-users")]
    public async Task<IActionResult> GetPendingUsers()
    {
        var user=await _adminService.GetPendingUsersAsync();
        return Ok(user);
    }

    [HttpPost("approve-user")]
    public async Task<IActionResult> ApproveUser([FromBody] ApproveUserDto dto)
    {
        var result=await _adminService.ApproveUserAsync(dto.UserId,dto.Role);

        if (!result)
        {
            return BadRequest("User could not be approved. Chack Id and Status");
        }

        return Ok(new
        {
            success=true,
            message="User approved Successfully"
        });
    }

    [HttpPost("reject-user")]
    public async Task<IActionResult> RejectUser(
        [FromBody] RejectUsersDto dto
    )
    {
        var result=await _adminService.RejectUserAsync(dto.UserId);

        if (!result)
        {
            return BadRequest("User could not be rejected");
        }

        return Ok(
            new
            {
                success=true,
                message="User rejected successfully"
            }
        );
    }

    
}