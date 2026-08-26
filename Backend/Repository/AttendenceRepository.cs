using Backend.Data;
using Backend.Interface;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class AttendenceRepository: IAttendenceRepository
{
    private readonly AppDbContext _context;

    public AttendenceRepository(AppDbContext context)
    {
        _context=context;
    }

    public async Task<Employee?>GetEmployeeByUserIdAsync(int userId)
    {
        return await _context.Employees
        .FirstOrDefaultAsync(e=>e.UserId==userId);
    }

    public async Task<Attendence?> GetTodayAttendenceAsync(int employeeId, DateOnly date)
    {
        return await _context.Attendences
        .FirstOrDefaultAsync(a=>a.EmployeeId==employeeId &&
        a.Date==date);

    }

    public async Task<IEnumerable<Attendence>>GetEmployeeAttendenceAsync(int employeeId)
    {
        return await _context.Attendences
        .Where(a=>a.EmployeeId==employeeId)
        .OrderByDescending(a=>a.Date)
        .ToListAsync();
    }

    public async Task<IEnumerable<Attendence>> GetEmployeeAttendenceByDateRangeAsync(
        int employeeId,
        DateOnly startDate,
        DateOnly endDate
    )
    {
        return await _context.Attendences
        .Where(a=>
        a.EmployeeId==employeeId &&
        a.Date>=startDate &&
        a.Date<=endDate)
        .OrderByDescending(a=>a.Date)
        .ToListAsync();
    }

    public async Task<IEnumerable<Attendence>> GetAllAttendencesAsync()
    {
        return await _context.Attendences
        .OrderByDescending(a=>a.Date)
        .ThenByDescending(a=>a.CheckIn)
        .ToListAsync();
    }

    public async Task<IEnumerable<Attendence>> GetAllAttendenceByDateRangeAsync(
        DateOnly startDate,
        DateOnly endDate
    )
    {
        return await _context.Attendences
        .AsNoTracking()
        .Where(a=>
            a.Date>=startDate &&
            a.Date<=endDate)
        .OrderByDescending(a=>a.Date)
        .ToListAsync();
    }


    public async Task AddAsync(Attendence attendence)
    {
        await _context.Attendences.AddAsync(attendence);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}