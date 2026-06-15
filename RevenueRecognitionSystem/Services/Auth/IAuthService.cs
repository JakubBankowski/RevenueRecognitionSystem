using RevenueRecognitionSystem.DTOs.Auth;

namespace RevenueRecognitionSystem.Services.Auth;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginRequestDto dto);
}