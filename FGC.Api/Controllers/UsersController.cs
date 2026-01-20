using Microsoft.AspNetCore.Mvc;
using FCG.Api.Application.DTOs;
using FCG.Api.Application.Services;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var userId = await _userService.CreateAsync(request);
            return CreatedAtAction(null, new { id = userId });
        }
    }
}
