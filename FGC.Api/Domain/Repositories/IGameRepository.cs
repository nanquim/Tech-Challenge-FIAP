using FCG.Api.Domain.Entities;

namespace FCG.Api.Domain.Repositories
{
    public interface IGameRepository
    {
        Task AddAsync(Game game);
        Task<Game?> GetByIdAsync(Guid id);
        Task<IEnumerable<Game>> GetAllAsync();
        Task UpdateAsync(Game game);
        Task DeleteAsync(Game game);
    }
}
