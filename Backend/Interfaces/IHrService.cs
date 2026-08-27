using Backend.Dtos.HR;
using Backend.Models.Enums;

namespace Backend.Interface;

public interface IHrService
{
    Task<IEnumerable<PendingLeaveDto>> GetAllLeaveRequests();
    Task<bool> ApproveLeave(int leaveId,int userId);

    Task<bool>RejectLeave(int leaveId,int userId);

}