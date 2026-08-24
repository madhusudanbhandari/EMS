using Backend.Data;
using Backend.Dtos.Attendence;
using Backend.Dtos.Auth;
using Backend.Interface;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Backend.Service;

public class AttendenceService : IAttendenceService
{
    private readonly AppDbContext _context;

    public AttendenceService(AppDbContext context)
    {
        _context=context;

    }

    public async Task<AttendenceResponseDto?>CheckInAsync(int userId)
    {

        var employee=await _context.Employees
        .FirstOrDefaultAsync(e=>e.UserId==userId);

        if (employee == null)
        {
            return null;
        }
        var today=DateOnly.FromDateTime(DateTime.UtcNow);

        var existingAttendence=await _context.Attendences
        .FirstOrDefaultAsync(a=>a.EmployeeId==employee.Id &&
        a.Date==today );

        if (existingAttendence != null)
        {
            throw new InvalidOperationException(
                "Employee has already checked in today"
            );
        }

        var now=DateTime.UtcNow;

        var attendence=new Attendence
        {
            EmployeeId=employee.Id,
            Date=today,
            CheckIn=now,
            Status=now.TimeOfDay> new TimeSpan(10,0,0)
            ? Models.Enums.AttendenceStatus.Late
            :Models.Enums.AttendenceStatus.Present
        };


        _context.Attendences.Add(attendence);
        await _context.SaveChangesAsync();

        return new AttendenceResponseDto
        {
            Id=attendence.Id,
            EmployeeId=attendence.EmployeeId,
            Date=attendence.Date,
            CheckIn=attendence.CheckIn,
            CheckOut=attendence.CheckOut,
            Status=attendence.Status.ToString()
        };


    }

    public async Task<AttendenceResponseDto?> CheckOutAsync(int userId)
    {
        var employee=await _context.Employees
        .FirstOrDefaultAsync(e=>e.UserId==userId);

        if (employee == null)
        {
            return null;
        }

        var today=DateOnly.FromDateTime(DateTime.UtcNow);

        var attendence=await _context.Attendences
        .FirstOrDefaultAsync(a=>
        a.EmployeeId==employee.Id &&
        a.Date==today);

        if (attendence == null)
        {
            throw new InvalidOperationException("You havent checked in today");
        }

        if (attendence.CheckOut != null)
        {
            throw new InvalidOperationException(
                "You have already checked out today"
            );
        }

        attendence.CheckOut=DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new AttendenceResponseDto
        {
            Id=attendence.Id,
            EmployeeId=attendence.EmployeeId,
            Date=attendence.Date,
            CheckIn=attendence.CheckIn,
            CheckOut=attendence.CheckOut,
            Status=attendence.Status.ToString()
        };
    }

    public async Task<IEnumerable<AttendenceResponseDto>> GetMyAttendenceAsync(int userId)
    {
        var employee=await _context.Employees
        .FirstOrDefaultAsync(e=>e.UserId==userId);

        if (employee == null)
        {
            return Enumerable.Empty<AttendenceResponseDto>();
        }

        var attendence=await _context.Attendences
        .Where(a=>a.EmployeeId==employee.Id)
        .OrderByDescending(a=>a.Date)
        .ToListAsync();

        return attendence.Select(a=>new AttendenceResponseDto
        {
            Id=a.Id,
            EmployeeId=a.EmployeeId,
            Date=a.Date,
            CheckIn=a.CheckIn,
            CheckOut=a.CheckOut,
            Status=a.Status.ToString()
        });
    }

    public async Task<IEnumerable<AttendenceResponseDto>> GetAllAttendenceAsync()
    {
        var attendences=await _context.Attendences
        .OrderByDescending(a=>a.Date)
        .ThenByDescending(a=>a.CheckIn)
        .ToListAsync();


        return attendences.Select(a=>new AttendenceResponseDto
        {
            Id=a.Id,
            EmployeeId=a.EmployeeId,
            Date=a.Date,
            CheckIn=a.CheckIn,
            CheckOut=a.CheckOut,
            Status=a.Status.ToString()
        });
    }
}