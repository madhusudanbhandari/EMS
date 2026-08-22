using Backend.Dtos.Admin;
using Backend.Models.Enums;

namespace Backend.Interface;
public interface IAdminService
{
    Task<IEnumerable<PendingUserResponseDto>>GetPendingUsersAsync();
    Task<bool> ApproveUserAsync(int userId,UserRole role);
    Task<bool> RejectUserAsync(int userId);
}