using Backend.Dtos.HR;

namespace Backend.Interface;

public interface IHrService
{
    Task<IEnumerable<PendingLeaveDto>> GetAllLeaveRequests();
    Task<bool> ApproveLeave(int leaveId,string status);

    Task<bool>RejectLeave(int leaveId,string status);

}