using Backend.Models;
using Backend.Models.Enums;

public class Leave
{
    public int Id{get;set;}
    public int EmployeeId{get;set;}
    public Employee employee{get;set;}=null!;
    public string LeaveType {get;set;}=string.Empty;
    public DateOnly startDate{get;set;}
    public DateOnly endDate{get;set;}   
    public string Reason{get;set;}=string.Empty;
    public LeaveStatus Status{get;set;}
    public DateTime AppliedAt{get;set;}
    public DateTime ReviewedAt{get;set;}
    public string ReviewedBy{get;set;}=string.Empty;



}