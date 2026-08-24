using Backend.Dtos.Attendence;

namespace Backend.Interface;

public interface IAttendenceService
{
    Task<AttendenceResponseDto?> CheckInAsync(int userId);
    Task<AttendenceResponseDto?> CheckOutAsync(int userId);
    Task<IEnumerable<AttendenceResponseDto>> GetMyAttendenceAsync(int userId);
    Task<IEnumerable<AttendenceResponseDto>> GetAllAttendenceAsync();

}