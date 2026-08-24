
namespace Backend.Dtos.Attendence;

public class AttendenceResponseDto
{
    public int Id{get;set;}
    public int EmployeeId{get;set;}
    public DateOnly Date{get;set;}
    public DateTime CheckIn{get;set;}
    public DateTime? CheckOut{get;set;}

    public string Status{get;set;}=string.Empty;
}