using FCG.Api.Domain.Entities;
using FCG.Api.Domain.Repositories;
using FCG.Api.Application.DTOs;

namespace FCG.Api.Application.Services
{
    public class GameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Guid> CreateAsync(CreateGameRequest request)
        {
            var game = new Game(
                request.Title,
                request.Description,
                request.Price
            );

            await _gameRepository.AddAsync(game);
            return game.Id;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _gameRepository.GetAllAsync();
        }
    }
}
