using Backend.Dtos.Common;
using Backend.Dtos.Employee;
using Backend.Models;

namespace Backend.Interface;

public interface IEmployeeService
{
    Task<PagedResponse<EmployeeResponeDto>> GetAllAsync(QueryParameters query);
    Task<EmployeeResponeDto?>GetByIdAsync(int id);
    Task<Employee?>CreateAsync(CreateEmployeeDto dto);
    Task<bool> UpdateAsync(int id,UpdateEmployeeDto dto);

    Task<bool>DeleteAsync(int id);

    Task<bool> CreateProfileAsync(int userId, CompleteEmployeeProfileDto dto);

    Task<EmployeeProfileDto?> GetMyProfile(int userID);

    Task<bool>UpdateMyProfile(int userId, UpdateEmployeeProfileDto dto);
}