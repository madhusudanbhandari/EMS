using Backend.Models.Enums;

namespace Backend.Dtos.Admin;

public class PendingUserResponseDto
{
    public int Id{get;set;}
    public string Name{get;set;}=string.Empty;
    public string Email{get;set;}=string.Empty;
    public AccountStatus Status{get;set;}
}