
namespace Backend.Dtos.Employee;

public class CreateEmployeeDto
{   
   public int Id{get;set;}
    public string FirstName{get;set;}=string.Empty;
    public string LastName{get;set;}=string.Empty;

    public string Email{get;set;}=string.Empty;

    public decimal Salary{get;set;}
    public int DepartmentId{get;set;}
}