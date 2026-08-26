
using Backend.Models.Enums;

namespace Backend.Dtos.HR;

public class RejectLeaveDto
{
    public int Id{get;set;}
    public LeaveStatus Status{get;set;}
}