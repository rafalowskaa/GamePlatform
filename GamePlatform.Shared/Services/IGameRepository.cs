using GamePlatform.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Shared.Services;

public interface IGameRepository
{
    /* ---- podstawowy CRUD ---- */
    Task<IReadOnlyList<Game>> GetAllAsync(string? filter = null);
    Task<Game?> GetAsync(Guid id);
    Task AddAsync(Game game, GameVersion firstVersion);
    Task DeleteAsync(Guid id);

    /* ---- wyniki gier ---- */
    Task AddScoreAsync(Guid gameId, Score score);

    /// <summary>Najlepsze wyniki (TOP N) danej gry.</summary>
    Task<IReadOnlyList<Score>> GetTopScoresAsync(Guid gameId, int limit);
}
