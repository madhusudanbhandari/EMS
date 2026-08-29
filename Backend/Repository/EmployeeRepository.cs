

using Backend.Data;
using Backend.Dtos.Employee;
using Backend.Dtos.HR;
using Backend.Interface;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context=context;
    }

    public IQueryable<Employee> GetAllEmployeesAsync()
    {
        return _context.Employees
        .AsNoTracking()
        .Include(e=>e.Department)
        .AsQueryable();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
        .Include(e=>e.Department)
        .FirstOrDefaultAsync(e=>e.Id==id);
    }

    public async Task<bool> DepartmentExistsAsync(int departmentId)
    {
        return await _context.Departments
        .AnyAsync(d=>d.Id==departmentId);
       
    }

    public async Task<Employee?> GetByUserIdAsync(int userId)
    {
        return await _context.Employees
        .FirstOrDefaultAsync(e=>e.UserId==userId);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users
        .FirstOrDefaultAsync(u=>u.Id==userId);
    }




    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
    }


    public void Remove(Employee employee)
    {
        _context.Employees.Remove(employee);
    }
    public async Task AddLeaveAsync(Leave leave)
    {
        _context.Leaves.Add(leave);
    }
     public async Task <IEnumerable<LeaveHistoryDto>> GetAllMyLeavesAsync(int employeeId)
    {
        return await _context.Leaves
        .AsNoTracking()
        .Where(l=>l.Employee.UserId==employeeId)
        .Select(l=>new LeaveHistoryDto
        {
            Id=l.Id,
            LeaveType=l.LeaveType,
            StartDate=l.StartDate,
            EndDate=l.EndDate,
            Reason=l.Reason,
            Status=l.Status,
            AppliedAt=l.AppliedAt,
            ReviewedAt=l.ReviewedAt,
            ReviewerName=l.Reviewer !=null
            ?l.Reviewer.Name
            : "not reviewd"

        }).ToListAsync();
        
    }

    public async Task<IEnumerable<MyPayrollDto>> GetMyPayroll(int userId)
    {
        

        return await _context.Payrolls
        .AsNoTracking()
        .Where(p=>p.Employee.UserId==userId)
        .Select(p=>new MyPayrollDto
        {
            Id=p.Id,
            PayrollPeriod=p.PayrollPeriod,
            BaseSalary=p.BaseSalary,
            Overtime=p.Overtime,
            Bonus=p.Bonus,
            GrossSalary=p.GrossSalary,
            TotalDeductions=p.TotalDeductions,
            NetSalary=p.NetSalary,
            Status=Models.Enums.SalaryStatus.released,
            ProcessedAt=p.ProcessedAt
        }
       ).ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}