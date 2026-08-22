using Backend.Models.Enums;

namespace Backend.Dtos.Admin;

public class ApproveUserDto
{
    public int UserId{get;set;}
    public UserRole Role{get; set;}

}
