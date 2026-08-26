
using System.Security.Claims;
using Backend.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttendenceController: ControllerBase
{
    private readonly IAttendenceService _attendenceService;

    public AttendenceController(IAttendenceService attendenceService)
    {
        _attendenceService=attendenceService;
    }

    [Authorize(Roles ="Employee")]
    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn()
    {
        var userIdClaim= User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new Exception("User is not logged in");
        }

        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("User is not logged in");
        }


        var checkIn=await _attendenceService.CheckInAsync(Id);

        return Ok(checkIn);

    }

    [Authorize(Roles ="Employee")]
    [HttpPost("check-out")]
    public async Task<IActionResult> CheckOut()
    {
       var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User is not logged in");
        } 

        if(!int.TryParse(userIdClaim.Value,out int Id))
        {
            return Unauthorized("User is not logged in");
        }

        var checkOut=await _attendenceService.CheckOutAsync(Id);
        return Ok(checkOut);
    }
    [Authorize(Roles ="Employee")]
    [HttpGet("my-attendence")]
    public async Task<IActionResult> MyAttendence()
    {
        var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User not logged in");
        }

        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("User not logged in");
        }

        var attendence=await _attendenceService.GetMyAttendenceAsync(Id);

        return Ok(attendence);
    }

    [Authorize(Roles ="Admin,HR")]
    [HttpGet("all-attendence")]
    public async Task<IActionResult> GetAllAttendence()
    {
        var attendences=await _attendenceService.GetAllAttendenceAsync();
        return Ok(attendences);
    }

    [Authorize(Roles ="Employee")]
    [HttpGet("my-attendence/range")]
    public async Task<IActionResult> MyAttendenceByDateRange(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate
    )
    {
        var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User not logged inn");
        }

        if(!int.TryParse(userIdClaim.Value, out int Id))
        {
            return Unauthorized("Invalid user");
        }

        if (startDate > endDate)
        {
            return BadRequest(
                "Start date cannot be after the end date"
            );
        }

        var attendence=await _attendenceService.
        GetMyAttendenceByDateRangeAsync(Id,
        startDate,
        endDate);

        return Ok(attendence);
    }

    [Authorize(Roles ="Employee")]
    [HttpGet("my-attendence/summary")]

    public async Task<IActionResult> MyAttendenceSummary(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate
    )
    {
        var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User not logged in");
        }

        if(!int.TryParse(userIdClaim.Value,out int Id))
        {
            return Unauthorized("Invalid user");
        }

        if (startDate >endDate)
        {
            return BadRequest(
                "Start date cannot be after end date"
            );
        }

        var summary=await _attendenceService
        .GetMyAttendenceSummaryAsync(
            Id,
            startDate,
            endDate
        );
        return Ok(summary);
    }

}