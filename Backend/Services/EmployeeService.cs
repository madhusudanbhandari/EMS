using Backend.Data;
using Backend.Dtos.Employee;
using Backend.Interface;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Backend.Dtos.Common;

namespace Backend.Service;


public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


        //Initial

    // public async Task<IEnumerable<EmployeeResponeDto>> GetAllAsync() { 
    //     return await _context.Employees.
    //     AsNoTracking().
    //     Select(e => new EmployeeResponeDto
    //      { 
    //         Id = e.Id, 
    //         FirstName = e.FirstName,
    //          LastName = e.LastName,
    //           Email = e.Email, 
    //           Salary = e.Salary, 
    //           DepartmentId = e.DepartmentId, 
    //           DepartmentName = e.Department!.Name
    //            }).
    //            ToListAsync();
    //             }


    //with AutoMapper

    // public async Task<IEnumerable<EmployeeResponeDto>> GetAllAsync()
    // {
    //     var employees=await _context.Employees
    //     .Include(e=>e.Department)
    //     .ToListAsync();

    //     return _mapper.Map<List<EmployeeResponeDto>>(employees);

    // }



    //Using AutoMapper+Pagination

    public async Task<PagedResponse<EmployeeResponeDto>> GetAllAsync(QueryParameters query)
    {
        var employeeQuery=_context.Employees
        .AsNoTracking()
        .Include(e=>e.Department)
        .AsQueryable();

        var totalCount=await employeeQuery.CountAsync();

        var employees=await employeeQuery
        .Skip((query.Page-1)*query.PageSize)
        .Take(query.PageSize)
        .ToListAsync();
        
        var employeeDtos=_mapper.Map<List<EmployeeResponeDto>>(employees);

        return new PagedResponse<EmployeeResponeDto>
        {
            Items=employeeDtos,
            Page=query.Page,
            PageSize=query.PageSize,
            TotalCount=totalCount,
            TotalPages=(int)Math.Ceiling(totalCount/(double)query.PageSize)
        };
    }



    
    public async Task<EmployeeResponeDto?> GetByIdAsync(int id)
    {
        var employee = await _context.Employees
        .Include(e => e.Department)
        .FirstOrDefaultAsync(e => e.Id == id);

        return _mapper.Map<EmployeeResponeDto>(employee);
    }
    

    public async Task<Employee?> CreateAsync(CreateEmployeeDto dto)
    {
        var departmentExists = await _context.Departments
        .AnyAsync(d => d.Id == dto.DepartmentId);

        if (!departmentExists)
        {
            return null;
        }

        var employee = _mapper.Map<Employee>(dto);

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;


    }

    public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return false;

        }

        var existingDepartment = await _context.Departments
        .AnyAsync(d => d.Id == dto.DepartmentId);

        if (!existingDepartment)
        {
            return false;
        }

        // employee.FirstName=dto.FirstName;
        // employee.LastName=dto.LastName;
        // employee.Email=dto.Email;
        // employee.Salary=dto.Salary;
        // employee.DepartmentId=dto.DepartmentId;

        _mapper.Map(dto, employee);

        await _context.SaveChangesAsync();
        return true;

    }


    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return false;
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return true;
    }
}