using Backend.Data;
using Backend.Dtos.Employee;
using Backend.Interface;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Backend.Dtos.Common;
using Backend.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Service;


public class EmployeeService : IEmployeeService
{   
    private readonly IEmployeeRepository _employeeRepository;

    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper,

    ICacheService cache)
    {
        _employeeRepository=employeeRepository;
        _mapper = mapper;
        _cache=cache;
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
        var employeeQuery= _employeeRepository.GetAllEmployeesAsync();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            employeeQuery=employeeQuery.Where(e=>
            e.FirstName.Contains(query.Search)||
            e.LastName.Contains(query.Search)||
            e.Email.Contains(query.Search));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            employeeQuery=query.SortBy.ToLower() switch
            {
                "firsname"=>query.SortOrder?.ToLower()=="desc"
                ?employeeQuery.OrderByDescending(e=>e.FirstName)
                :employeeQuery.OrderBy(e=>e.FirstName),

                "lastname"=>query.SortOrder?.ToLower()=="desc"
                ?employeeQuery.OrderByDescending(e=>e.LastName)
                :employeeQuery.OrderBy(e=>e.LastName),

                "email"=>query.SortOrder?.ToLower()=="desc"
                ?employeeQuery.OrderByDescending(e=>e.Email)
                :employeeQuery.OrderBy(e=>e.Email),

                "salary"=>query.SortOrder?.ToLower()=="desc"
                ?employeeQuery.OrderByDescending(e=>e.Salary)
                :employeeQuery.OrderBy(e=>e.Salary),

                _ => employeeQuery.OrderBy(e => e.Id)

            };
        }
        else
        {
            employeeQuery=employeeQuery.OrderBy(e=>e.Id);
        }

        if (query.DepartmentId.HasValue)
        {
            employeeQuery=employeeQuery.Where(e=>
            e.DepartmentId==query.DepartmentId.Value);
        }

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
        var employee = await _employeeRepository.GetByIdAsync(id);
        return _mapper.Map<EmployeeResponeDto>(employee);
    }
    

    public async Task<Employee?> CreateAsync(CreateEmployeeDto dto)
    {
        var departmentExists = await _employeeRepository.DepartmentExistsAsync(dto.DepartmentId);
        
        if (!departmentExists)
        {
            return null;
        }

        var employee = _mapper.Map<Employee>(dto);

         await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        return employee;


    }

    public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return false;

        }

        var existingDepartment = await _employeeRepository.DepartmentExistsAsync(dto.DepartmentId);

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

        await _employeeRepository.SaveChangesAsync();
        return true;

    }


    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return false;
        }

        _employeeRepository.Remove(employee);
        await _employeeRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CreateProfileAsync(int userId,CompleteEmployeeProfileDto dto)
    {
        var user=await _employeeRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        if (user.Status != Models.Enums.AccountStatus.Approved)
        {
            return false;
        }

        if (user.Role != UserRole.Employee)
        {
           return false; 
        }

        var existingProfile=await _employeeRepository.GetByUserIdAsync(userId);

        if (existingProfile != null)
        {
            return false;
        }

        var departmentExists=await _employeeRepository.DepartmentExistsAsync(dto.DepartmentId);

        if (!departmentExists)
        {
            return false;
        }

        string? ProfilePicture=null;

        if(dto.ProfilePicture!=null &&
        dto.ProfilePicture.Length > 0)
        {
            ProfilePicture=await SaveProfilePictureAsync(
                dto.ProfilePicture
            );
        }


        var employee=new Models.Employee
        {
            FirstName=dto.FirstName,
            LastName=dto.LastName,
            Email=dto.Email,
            Salary=dto.Salary,
            DepartmentId=dto.DepartmentId,
            UserId=userId,
            ProfilePicture=ProfilePicture

            


        };
        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        return true;
    }


    // public async Task<EmployeeProfileDto?> GetMyProfile(int userId)
    // {
    //     var user=await _context.Employees
    //     .FirstOrDefaultAsync(e=>e.UserId==userId);

    //   

    //     if (user == null)
    //     {
    //         return null;
    //     }

    //     return new EmployeeProfileDto
    //     {
    //         FirstName=user.FirstName,
    //         LastName=user.LastName,
    //         Email=user.Email,
    //         Salary=user.Salary,
    //         DepartmentId=user.DepartmentId,
    //         ProfilePicture=user.ProfilePicture,
            

    //     };

      
    // }

    public async Task<EmployeeProfileDto?> GetMyProfile(int userId)
    {
        var cacheKey=$"employee-profile:{userId}";

        var cachedProfile=await _cache.GetAsync<EmployeeProfileDto>(cacheKey);

        if (cachedProfile != null)
        {
            return cachedProfile;
        }

        var employee=await _employeeRepository.GetByUserIdAsync(userId);

        if (employee == null)
        {
            return null;
        }

        var profile=new EmployeeProfileDto
        {
            FirstName=employee.FirstName,
            LastName=employee.LastName,
            Email=employee.Email,
            Salary=employee.Salary,
            DepartmentId=employee.DepartmentId,
            ProfilePicture=employee.ProfilePicture
        };

        await _cache.SetAsync(
            cacheKey,
            profile,
            TimeSpan.FromMinutes(10)
        );

        return profile;


    }






    public async Task<bool> UpdateMyProfile(int userId, [FromForm]UpdateEmployeeProfileDto dto)
    {
        var employee=await _employeeRepository.GetByUserIdAsync(userId);

        if (employee == null)
        {
            return false;
        }

        employee.FirstName=dto.FirstName;
        employee.LastName=dto.LastName;
        employee.Salary=dto.Salary;
        employee.Email=dto.Email;
        employee.DepartmentId=dto.DepartmentId;

        if(dto.ProfilePicture!=null &&
        dto.ProfilePicture.Length > 0)
        {
            var profilePicture=await SaveProfilePictureAsync(dto.ProfilePicture);

            employee.ProfilePicture=profilePicture;


        }

         await _employeeRepository.SaveChangesAsync();
         await _cache.RemoveAsync($"employee-profile:{userId}");
         

         return true;

    }


    //Image Saving Method

    private async Task<string> SaveProfilePictureAsync(IFormFile file)
    {
        var allowedExtensions = new[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        var extension=Path.GetExtension(file.FileName)
        .ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            throw new ArgumentException(
                "Only JPG, JPEG, PNG, and WEBP images are allowed"
            );
        }

        const long maxFileSize=20*1024*1024;

        if (file.Length > maxFileSize)
        {
            throw new ArgumentException("Profiles picture cannot exceed 5 MB");
        }

        var uploadsFolder=Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "profiles"
        );

        var fileName=$"{Guid.NewGuid()}{extension}";

        var filePath=Path.Combine(
            uploadsFolder,
            fileName
        );

        await using var stream=new FileStream(
            filePath,
            FileMode.Create
        );

        await file.CopyToAsync(stream);

        return $"/uploads/profiles/{fileName}";
    }


    public async Task<LeaveResponseDto?> ApplyLeave(int userId, ApplyLeaveDto dto)
    {
        var employee=await _employeeRepository.GetByUserIdAsync(userId);

        if (employee == null)
        {
            throw new Exception("User not logged in");
        }

        if (dto.startDate > dto.endDate)
        {
            throw new InvalidOperationException("Start date cant be after end date");
        }

        var newLeave= new Leave
        {
            LeaveType=dto.LeaveType,
            StartDate=dto.startDate,
            EmployeeId=employee.Id,
            EndDate=dto.endDate,
            Reason=dto.Reason,

        };

        var result=new LeaveResponseDto
        {
            EmployeeName=employee.FirstName,
            LeaveType=newLeave.LeaveType,
            startDate=newLeave.StartDate,
            endDate=newLeave.EndDate,
            Reason=newLeave.Reason,
            Status=LeaveStatus.Pending,
            AppliedAt=DateTime.UtcNow,
            

        };

        await _employeeRepository.AddLeaveAsync(newLeave);
        await _employeeRepository.SaveChangesAsync();
        return result;        

    }

    public async Task<IEnumerable<LeaveHistoryDto>> GetAllMyLeavesAsync(int userId)
    {
        return await _employeeRepository.GetAllMyLeavesAsync(userId);
    }
}