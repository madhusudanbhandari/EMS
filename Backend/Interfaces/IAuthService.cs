

using Backend.Dtos.Auth;

namespace Backend.Interface;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<CurrentUserDto?> GetCurrentUserAsync(int userId);
}