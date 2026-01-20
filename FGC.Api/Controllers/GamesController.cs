using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FCG.Api.Application.DTOs;
using FCG.Api.Application.Services;

namespace FCG.Api.Controllers
{
    [ApiController]
    [Route("games")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;

        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        // 🔒 ADMIN ONLY
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateGameRequest request)
        {
            var gameId = await _gameService.CreateAsync(request);
            return CreatedAtAction(null, new { id = gameId });
        }

        // 🔓 USER / ADMIN
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var games = await _gameService.GetAllAsync();
            return Ok(games);
        }
    }
}
