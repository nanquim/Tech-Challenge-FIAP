using Microsoft.EntityFrameworkCore;
using FCG.Api.Domain.Entities;
using FCG.Api.Domain.Repositories;
using FCG.Api.Infrastructure.Persistence.Contexts;

namespace FCG.Api.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task<Game?> GetByIdAsync(Guid id)
            => await _context.Games.FindAsync(id);

        public async Task<IEnumerable<Game>> GetAllAsync()
            => await _context.Games.ToListAsync();

        public async Task UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
}
