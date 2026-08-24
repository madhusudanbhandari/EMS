using Backend.Data;
using Backend.Dtos.Attendence;
using Backend.Dtos.Auth;
using Backend.Interface;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Backend.Service;

public class AttendenceService : IAttendenceService
{
    private readonly IAttendenceRepository _attendenceRepository;
    public AttendenceService(IAttendenceRepository attendenceRepository)
    {
        _attendenceRepository=attendenceRepository;

    }

    public async Task<AttendenceResponseDto?>CheckInAsync(int userId)
    {

        var employee=await _attendenceRepository.GetEmployeeByUserIdAsync(userId);

        if (employee == null)
        {
            return null;
        }
        var today=DateOnly.FromDateTime(DateTime.UtcNow);

        var existingAttendence=await _attendenceRepository.GetTodayAttendenceAsync(
            employee.Id,
            today
        );


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


         await _attendenceRepository.AddAsync(attendence);
         await _attendenceRepository.SaveChangesAsync();

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
        var employee=await _attendenceRepository.GetEmployeeByUserIdAsync(userId);

        if (employee == null)
        {
            return null;
        }

        var today=DateOnly.FromDateTime(DateTime.UtcNow);

        var attendence=await _attendenceRepository.GetTodayAttendenceAsync(employee.Id,today);

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

        await _attendenceRepository.SaveChangesAsync();

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
        var employee=await _attendenceRepository.GetEmployeeByUserIdAsync(userId);

        if (employee == null)
        {
            return Enumerable.Empty<AttendenceResponseDto>();
        }

        var attendence=await _attendenceRepository.GetEmployeeAttendenceAsync(employee.Id);
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
        var attendences=await _attendenceRepository.GetAllAttendencesAsync();


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