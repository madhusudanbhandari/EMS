using Backend.Dtos.Employee;
using Backend.Models;

namespace Backend.Interface;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponeDto>> GetAllAsync();
    Task<EmployeeResponeDto?>GetByIdAsync(int id);
    Task<Employee?>CreateAsync(CreateEmployeeDto dto);
    Task<bool> UpdateAsync(int id,UpdateEmployeeDto dto);

    Task<bool>DeleteAsync(int id);
}