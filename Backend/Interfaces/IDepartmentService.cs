using Backend.Dtos.Department;

namespace Backend.Interface;

public interface IDepartmentService
{
    Task<IEnumerable<Models.Department>> GetAllAsync();
    Task<Models.Department?> GetByIdAsync(int id);
    Task<Models.Department> CreateAsync(CreateDepartmentDto dto);

    Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto);
    Task<bool> DeleteAsync(int id);
}