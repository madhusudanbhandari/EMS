
namespace Backend.Models;

public class UpdateEmployeeDto
{   
    public int Id{get;set;}
    public string FirstName{get;set;}=string.Empty;
    public string LastName{get;set;}=string.Empty;

    public string Email{get;set;}=string.Empty;

    public int Salary{get;set;}
    public int DepartmentId{get;set;}

    public IFormFile? ProfilePicture{get;set;}
}