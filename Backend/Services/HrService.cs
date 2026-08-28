
using Backend.Data;
using Backend.Dtos.HR;
using Backend.Interface;
using Backend.Models;
using Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service;

public class HrService:IHrService
{
    private readonly AppDbContext _context;
    private readonly IEmployeeRepository _employeeRepository;

    public HrService(AppDbContext context, IEmployeeRepository employeeRepository)
    {
        _context=context;
        _employeeRepository=employeeRepository;
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
            EmployeeName=l.Employee.FirstName+" "+l.Employee.LastName,
            startDate=l.StartDate,
            endDate=l.EndDate,
            LeaveType=l.LeaveType,
            Reason=l.Reason,
            AppliedAt=l.AppliedAt

            

        }).ToListAsync();        
    }

    public async Task<bool> ApproveLeave(int leaveId,int reviewerId)
    {
        var leaveRequest=await _context.Leaves
        .FirstOrDefaultAsync(l=>l.Id==leaveId);

        if (leaveRequest == null)
        {
            return false;
        }

        if (leaveRequest.Status != LeaveStatus.Pending)
        {
             return false;
        }

        leaveRequest.Status=LeaveStatus.Approved;
        leaveRequest.ReviewedBy=reviewerId;
        leaveRequest.ReviewedAt=DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectLeave(int leaveId, int reviewerId)
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
        leaveRequest.Status=Models.Enums.LeaveStatus.Rejected;
        leaveRequest.ReviewedBy=reviewerId;
        leaveRequest.ReviewedAt=DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PayrollResponseDto> CreatePayroll(int employeeId,CreatePayrollDto dto)
    {
        var employee=_employeeRepository.GetByUserIdAsync(employeeId);

        if (employee== null)
        {
            throw new Exception("Employee does not exists");
        }

        var payroll=new Payroll
        {
            
            EmployeeId=employee.Id,
            PayrollPeriod=dto.PayrollPeriod,
            BaseSalary=dto.BaseSalary,
            Overtime=dto.Overtime,
            Bonus=dto.Bonus,
            GrossSalary=dto.GrossSalary,
            TotalDeductions=dto.TotalDeductions,
            NetSalary=dto.NetSalary,

            Status=SalaryStatus.pending,
            ProcessedAt=DateTime.UtcNow,
        };

        _context.Payrolls.Add(payroll);
        await _context.SaveChangesAsync();

        return new PayrollResponseDto
        {
            Id=payroll.Id,
            EmployeeId=payroll.EmployeeId,
            PayrollPeriod=payroll.PayrollPeriod,
            BaseSalary=payroll.BaseSalary,
            Overtime=payroll.Overtime,
            Bonus=payroll.Bonus,
            GrossSalary=payroll.GrossSalary,
            TotalDeductions=payroll.TotalDeductions,
            NetSalary=payroll.NetSalary,
            Status=payroll.Status,
            ProcessedAt=payroll.ProcessedAt,
        };
    }

}