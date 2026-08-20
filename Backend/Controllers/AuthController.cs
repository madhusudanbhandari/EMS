
using Backend.Dtos.Auth;
using Backend.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService=authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
           var user=await _authService.RegisterAsync(dto);
           return Ok(user); 
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpPost("login")]
    public async Task<IActionResult>Login(LoginDto dto)
    {
        var user=await _authService.LoginAsync(dto);
        
        if (user == null)
        {
            return Unauthorized("Login Failed");
        }
            return Ok(user);
      
    }


}