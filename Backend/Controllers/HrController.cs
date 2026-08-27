
using System.Security.Claims;
using Backend.Dtos.HR;
using Backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Backend.Controller;

[ApiController]
[Route("api/[controller]")]
public class HrController : ControllerBase
{
    private readonly IHrService _hrService;
    public HrController(IHrService hrService)
    {
        _hrService=hrService;
    }

    [Authorize(Roles ="HR")]
    [HttpGet("pending-leaves")]
    public async Task<IActionResult> GetAllLeaveRequests()
    {
        var requests=await _hrService.GetAllLeaveRequests();
        return Ok(requests);
    }

    [Authorize(Roles ="HR")]
    [HttpPost("approve-leave")]
    public async Task<IActionResult> ApproveLeave([FromBody]ApproveLeaveDto dto)
    {
        var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User is not logged in");
        }

        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("User not loggeed");
        }

        var result=await _hrService.ApproveLeave(dto.Id,Id);
        return Ok(result);
    }

    [Authorize(Roles ="HR")]
    [HttpPost("reject-leave")]
    public async Task<IActionResult> RejectLeave([FromBody] RejectLeaveDto dto)
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

        var result=await _hrService.RejectLeave(dto.Id,Id);
        return Ok(result);
    }

}