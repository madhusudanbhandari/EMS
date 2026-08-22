using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Employee;

public class CompleteEmployeeProfileDto
{
    public string FirstName{get;set;}=string.Empty;
    public string LastName{get;set;}=string.Empty;
    public  string Phone{get;set;}=string.Empty;

    public string Address{get;set;}=string.Empty;

    public string Email{get;set;}=string.Empty;

    public decimal Salary{get;set;}
    public int DepartmentId{get;set;}

    public IFormFile? ProfilePicture{get;set;}
}