using FinApp.Core.DTOs.Auth;

namespace FinApp.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<bool> RegisterAsync(RegisterRequestDto request);
    Task<bool> ValidateTokenAsync(string token);
}
