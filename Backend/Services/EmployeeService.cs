using Backend.Data;
using Backend.Dtos.Employee;
using Backend.Interface;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service;


public class EmployeeService: IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context=context;
    }


    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
        .Include(e=>e.Department)
        .AsNoTracking().
        ToListAsync();
     
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
        .Include(e=>e.Department)
        .AsNoTracking()
        .FirstOrDefaultAsync(e=>e.Id==id);
    }

    public async Task<Employee?> CreateAsync(CreateEmployeeDto dto)
    {
        var departmentExists=await _context.Departments
        .AnyAsync(d=>d.Id==dto.DepartmentId);

        if (!departmentExists)
        {
            return null;
        }

        var employee=new Employee
        {
            FirstName=dto.FirstName,
            LastName=dto.LastName,
            Email=dto.Email,
            Salary=dto.Salary,
            DepartmentId=dto.DepartmentId

        };

    _context.Employees.Add(employee);
    await _context.SaveChangesAsync();
    return employee;

        
    }

    public async Task<bool> UpdateAsync(int id,UpdateEmployeeDto dto)
    {
        var employee=await _context.Employees
        .FirstOrDefaultAsync(e=>e.Id==id);

        if (employee == null)
        {
            return false;

        }

        var existingDepartment=await _context.Departments
        .AnyAsync(d=>d.Id==dto.DepartmentId);

        if (!existingDepartment)
        {
            return false;
        }

        employee.FirstName=dto.FirstName;
        employee.LastName=dto.LastName;
        employee.Email=dto.Email;
        employee.Salary=dto.Salary;
        employee.DepartmentId=dto.DepartmentId;

        await _context.SaveChangesAsync();
        return true;
        
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var employee=await _context.Employees
        .FirstOrDefaultAsync(e=>e.Id==id);

        if (employee==null)
        {
            return false;
        }

         _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return true;
    }
}