using Backend.Models;
using Backend.Models.Enums;

public class ApplyLeaveDto
{
    public string LeaveType {get;set;}=string.Empty;
    public DateOnly startDate{get;set;}
    public DateOnly endDate{get;set;}   
    public string Reason{get;set;}=string.Empty;



}