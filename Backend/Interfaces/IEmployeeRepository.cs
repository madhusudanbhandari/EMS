
using Backend.Models;

namespace Backend.Interface;

public interface IEmployeeRepository
{
     public IQueryable<Employee> GetAllEmployeesAsync();
    Task<Employee?> GetByIdAsync(int userId);
    Task<bool> DepartmentExistsAsync(int departmentId);
    Task<Employee?> GetByUserIdAsync(int userId); 

    Task<User?> GetUserByIdAsync(int userId);

    Task AddAsync(Employee employee);

    Task AddLeaveAsync(Leave leave);
    void Remove(Employee employee);
    Task SaveChangesAsync();




}