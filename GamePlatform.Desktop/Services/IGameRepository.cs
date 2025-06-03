using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public interface IGameRepository
{
    /* ---- podstawowy CRUD ---- */
    Task<IReadOnlyList<Game>> GetAllAsync(string? filter = null);
    Task<Game?> GetAsync(Guid id);
    Task AddAsync(Game game, GameVersion firstVersion);

    Task UpdateAsync(Game game);
    Task<IReadOnlyList<Game>> GetPendingAsync();
    Task DeleteAsync(Guid id);

    Task ToggleFavoriteAsync(Guid userId, Guid gameId);
    Task<IReadOnlyList<Game>> GetFavoritesAsync(Guid userId);

    Task AddPlayTimeAsync(Guid userId, Guid gameId, int seconds);


    /* ---- wyniki gier ---- */
    Task AddScoreAsync(Guid gameId, Score score);

    /// <summary>Najlepsze wyniki (TOP N) danej gry.</summary>
    Task<IReadOnlyList<Score>> GetTopScoresAsync(Guid gameId, int limit);
}
