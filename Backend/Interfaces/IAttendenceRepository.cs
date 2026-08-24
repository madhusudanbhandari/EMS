using Backend.Models;

namespace Backend.Interface;

public interface IAttendenceRepository
{
    Task<Employee?>GetEmployeeByUserIdAsync(int userId);

    Task<Attendence?> GetTodayAttendenceAsync(int employeeId, DateOnly date);

    Task<IEnumerable<Attendence>> GetEmployeeAttendenceAsync(
        int employeeId
    );

    Task<IEnumerable<Attendence>> GetAllAttendencesAsync();
    Task AddAsync(Attendence attendence);

    Task SaveChangesAsync();
}