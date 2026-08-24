

using Backend.Data;
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

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


}