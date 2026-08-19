using Backend.Data;
using Backend.Dtos.Department;
using Backend.Interface;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _context;

    public DepartmentService(AppDbContext context)
    {
        _context=context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
        .AsNoTracking()
        .FirstOrDefaultAsync(d=>d.Id==id);
    }

    public async Task<Department> CreateAsync(CreateDepartmentDto dto)
    {
        var department=new Department
        {
            Name=dto.Name
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return department;


    }

    public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department= await _context.Departments
        .FirstOrDefaultAsync(d=>d.Id==id);

        if(department==null)
        {
            return false;
            }

        department.Name=dto.Name;

        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department= await _context.Departments
        .FirstOrDefaultAsync(d=>d.Id==id);

        if (department == null)
        {
            return false;
        }

        _context.Departments.Remove(department);

        await _context.SaveChangesAsync();
        return true;
    }

}