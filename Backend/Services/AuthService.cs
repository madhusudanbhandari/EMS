
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.Dtos.Auth;
using Backend.Interface;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Service;

public class AuthService: IAuthService
{
    private readonly AppDbContext _context;

    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context=context;
        _configuration=configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser=await _context.Users
        .FirstOrDefaultAsync(u=>u.Email==dto.Email);

        if (existingUser!=null)
        {
            throw  new Exception("Email already exist");
        }
        if (dto.Password != dto.ConfirmPassword)
        {
            throw new Exception("Passwords did not match");
        }

        var passwordHash= BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user=new User
        {
            Name=dto.Name,
            Email=dto.Email,
            PasswordHash=passwordHash,
            Role="Employee"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new AuthResponseDto
        {
            UserId=user.Id,
            FullName=user.Name,
            Email=user.Email,
            Role=user.Role
            
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user=await _context.Users
        .FirstOrDefaultAsync(u=>u.Email==dto.Email);

        if (user == null)
        {
            return null;
        }


        var passwordValid=BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash
        );

        if (!passwordValid)
        {
            return null;
        }

        var token=GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token=token,
            UserId=user.Id,
            FullName=user.Name,
            Email=user.Email,
            Role=user.Role
        };
        
    }

    private string GenerateJwtToken(User user)
    {
        var key=new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials=new SigningCredentials(key,
        SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };


        var token=new JwtSecurityToken(
            issuer:_configuration["Jwt:Issuer"],
            audience:_configuration["Jwt:Audience"],
            claims:claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
            ),
            signingCredentials:credentials


        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}



