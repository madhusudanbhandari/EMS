using Backend.Dtos;
using Backend.Models.Enums;

namespace Backend.Dtos.HR;
public class PendingLeaveDto
{
    public int Id{get;set;}
    public int EmployeeId{get;set;}
    public DateOnly startDate{get;set;}
    public DateOnly endDate{get;set;}   
    public LeaveStatus Status{get;set;}

}