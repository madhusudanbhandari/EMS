
namespace Backend.Dtos.Employee;

public class EmployeeResponeDto
{   
    public int Id{get;set;}
    public string FirstName{get;set;}=string.Empty;
    public string LastName{get;set;}=string.Empty;

    public string Email{get;set;}=string.Empty;

    public decimal Salary{get;set;}
    public int DepartmentId{get;set;}

    public string DepartmentName {get;set;}=string.Empty;
}