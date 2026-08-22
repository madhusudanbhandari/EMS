using Backend.Models.Enums;

namespace Backend.Dtos.Auth;

public class AuthResponseDto
{
    public string? Token{get;set;}
    public int UserId{get;set;}
    public string FullName{get;set;}=string.Empty;
    public string Email{get;set;}=string.Empty;
    public AccountStatus Status{get;set;}
    public UserRole Role{get;set;}

    public string Message{get ;set;}=string.Empty;

   


}