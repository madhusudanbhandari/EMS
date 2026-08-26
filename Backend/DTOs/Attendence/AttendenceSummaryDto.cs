namespace Backend.Dtos.Attendence;

public class AttendenceSummaryDto
{
    public int TotalDays{get;set;}
    public int PresentDays{get;set;}
    public int LateDays{get;set;}
    public int AbsentDays{get;set;}
    public double TotalWorkingHours{get;set;}
}