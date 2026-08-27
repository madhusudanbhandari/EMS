using Backend.Models.Enums;

namespace Backend.Dtos.Employee;
public class LeaveResponseDto
{
    public string LeaveType {get;set;}=string.Empty;
    public DateOnly startDate{get;set;}
    public DateOnly endDate{get;set;}   

    public string EmployeeName{get;set;}=string.Empty;
    public string Reason{get;set;}=string.Empty;
    public LeaveStatus Status{get;set;}
    public DateTime AppliedAt{get;set;}
    public DateTime? ReviewedAt{get;set;}
    public int? ReviewedBy{get;set;}



}