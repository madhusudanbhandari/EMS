
namespace Backend.Models;

public class Employee
{   
    public int Id{get;set;}
    public string FirstName{get;set;}=string.Empty;
    public string LastName{get;set;}=string.Empty;

    public string Email{get;set;}=string.Empty;

    public decimal Salary{get;set;}
    public int DepartmentId{get;set;}
    public Department? Department{get;set;}

    public int UserId{get;set;}
    public User User{get;set;}=null!;
}