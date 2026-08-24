using Backend.Models.Enums;

namespace Backend.Models;

public class Attendence
{
    public int Id{get;set;}
    public int EmployeeId{get;set;}
    public DateOnly Date{get;set;}
    public DateTime CheckIn{get;set;}
    public DateTime? CheckOut{get;set;}
    public AttendenceStatus Status{get;set;}
    public Employee Employee{get;set;}=null!;
}