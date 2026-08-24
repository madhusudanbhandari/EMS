
using Backend.Models;

namespace Backend.Interface;

public interface IEmployeeRepository
{
     public IQueryable<Employee> GetAllEmployeesAsync();
    Task<Employee?> GetByIdAsync(int userId);
    Task<bool> DepartmentExistsAsync(int departmentId);
    Task<User?> GetUserByIdAsync(int userId); 

    Task AddAsync(Employee employee);
    void Remove(Employee employee);
    Task SaveChangesAsync();




}