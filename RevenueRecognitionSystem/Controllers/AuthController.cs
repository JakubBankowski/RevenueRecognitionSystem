using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.DTOs.Auth;
using RevenueRecognitionSystem.Services.Auth;

namespace RevenueRecognitionSystem.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var token = await _authService.LoginAsync(dto);

        if (token == null)
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        return Ok(new { Token = token });
    }
}