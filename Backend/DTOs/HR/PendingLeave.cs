using Backend.Dtos;
using Backend.Models.Enums;

namespace Backend.Dtos.HR;
public class PendingLeaveDto
{
    public int Id{get;set;}
    public int EmployeeId{get;set;}
    public string LeaveType{get;set;}=string.Empty;
    public string EmployeeName{get;set;}=string.Empty;
    public DateOnly startDate{get;set;}
    public DateOnly endDate{get;set;}   
    public LeaveStatus Status{get;set;}
    public string Reason{get;set;}=string.Empty;
    public DateTime AppliedAt{get;set;}

}