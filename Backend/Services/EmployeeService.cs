using Backend.Data;
using Backend.Dtos.Employee;
using Backend.Interface;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Backend.Service;


public class EmployeeService: IEmployeeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeService(AppDbContext context,IMapper mapper)
    {
        _context=context;
        _mapper=mapper;
    }


    public async Task<IEnumerable<EmployeeResponeDto>> GetAllAsync()
    {
        var employees=await _context.Employees
        .Include(e=>e.Department)
        .ToListAsync();

        return _mapper.Map<List<EmployeeResponeDto>>(employees);
     
    }


    


    public async Task<EmployeeResponeDto?> GetByIdAsync(int id)
    {
        var employee=await _context.Employees
        .Include(e=>e.Department)
        .FirstOrDefaultAsync(e=>e.Id==id);

        return _mapper.Map<EmployeeResponeDto>(employee);
    }

    public async Task<Employee?> CreateAsync(CreateEmployeeDto dto)
    {
        var departmentExists=await _context.Departments
        .AnyAsync(d=>d.Id==dto.DepartmentId);

        if (!departmentExists)
        {
            return null;
        }

        var employee=_mapper.Map<Employee>(dto);

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

        // employee.FirstName=dto.FirstName;
        // employee.LastName=dto.LastName;
        // employee.Email=dto.Email;
        // employee.Salary=dto.Salary;
        // employee.DepartmentId=dto.DepartmentId;

        _mapper.Map(dto,employee);

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