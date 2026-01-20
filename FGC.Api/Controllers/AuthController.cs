using Microsoft.AspNetCore.Mvc;
using FCG.Api.Application.DTOs;
using FCG.Api.Application.Services;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            return Ok(new { token });
        }
    }
}
