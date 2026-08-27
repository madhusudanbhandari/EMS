
using Backend.Models.Enums;

namespace Backend.Dtos.Employee;

public class LeaveHistoryDto
{
    public int Id{get;set;}
    public string LeaveType {get;set;}=string.Empty;

    public DateOnly StartDate{get;set;}
    public DateOnly EndDate{get;set;}
    public string Reason{get;set;}=string.Empty;
    public LeaveStatus Status{get;set;}
    public DateTime AppliedAt{get;set;}
    public DateTime? ReviewedAt{get;set;}
    public string ReviewerName{get;set;}=string.Empty;
}