using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Desktop.Services;

/// <summary>
/// Implementacja repozytorium gier oparta na SQLite + EF Core.
/// </summary>
public sealed class GameRepositorySqlite : IGameRepository
{
    private readonly AppDbContext _db;
    public GameRepositorySqlite(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Game>> GetAllAsync(string? filter = null)
    {
        IQueryable<Game> q = _db.Games
                                .Include(g => g.Versions)
                                .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter))
            q = q.Where(g => g.Title.Contains(filter));

        return await q.ToListAsync();
    }

    public async Task<Game?> GetAsync(Guid id) =>
        await _db.Games
                 .Include(g => g.Versions)
                 .Include(g => g.Scores)
                 .FirstOrDefaultAsync(g => g.Id == id);

    public async Task AddAsync(Game game, GameVersion firstVersion)
    {
        game.Versions.Add(firstVersion);
        _db.Games.Add(game);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _db.Games.Remove(new Game { Id = id });
        await _db.SaveChangesAsync();
    }

    public async Task AddScoreAsync(Guid gameId, Score score)
    {
        var game = await _db.Games.FindAsync(gameId);
        if (game is null) return;

        game.Scores.Add(score);
        await _db.SaveChangesAsync();
    }
}
