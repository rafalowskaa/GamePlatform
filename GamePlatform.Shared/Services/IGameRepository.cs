using GamePlatform.Shared.Models;

namespace GamePlatform.Shared.Services;

public interface IGameRepository
{
    Task<IReadOnlyList<Game>> GetAllAsync(string? filter = null);
    Task<Game?> GetAsync(Guid id);
    Task AddAsync(Game game, GameVersion firstVersion);
    Task DeleteAsync(Guid id);
    Task AddScoreAsync(Guid gameId, Score score);
}