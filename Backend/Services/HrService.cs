
using Backend.Data;
using Backend.Dtos.HR;
using Backend.Interface;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service;

public class HrService:IHrService
{
    private readonly AppDbContext _context;

    public HrService(AppDbContext context)
    {
        _context=context;
    }

    public async Task<IEnumerable<PendingLeaveDto>> GetAllLeaveRequests()
    {
        return await _context.Leaves.
        AsNoTracking()
        .Where(l=>l.Status==Models.Enums.LeaveStatus.Pending)
        .Select(l=>new PendingLeaveDto
        {
            Id=l.Id,
            Status=l.Status,
            EmployeeId=l.EmployeeId,
            startDate=l.startDate,
            endDate=l.endDate

        }).ToListAsync();        
    }

    public async Task<bool> ApproveLeave(int leaveId,string status)
    {
        var leaveRequest=await _context.Leaves
        .FirstOrDefaultAsync(l=>l.Id==leaveId);

        if (leaveRequest == null)
        {
            return false;
        }

        if (leaveRequest.Status != Models.Enums.LeaveStatus.Pending)
        {
             return false;
        }

        leaveRequest.Status=Models.Enums.LeaveStatus.Approved;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectLeave(int leaveId,string status)
    {
        var leaveRequest=await _context.Leaves
        .FirstOrDefaultAsync(l=>l.Id==leaveId);

        if (leaveRequest == null)
        {
            return false;
            
        }
    }

}