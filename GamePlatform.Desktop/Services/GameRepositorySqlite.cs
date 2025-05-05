using GamePlatform.Shared.Data;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Desktop.Services;

public class GameRepositorySqlite(AppDbContext db) : IGameRepository
{
    public async Task<IReadOnlyList<Game>> GetAllAsync(string? filter = null)
    {
        var q = db.Games.Include(g => g.Versions).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filter)) q = q.Where(g => g.Title.Contains(filter));
        return await q.ToListAsync();
    }

    public async Task<Game?> GetAsync(Guid id) =>
        await db.Games.Include(g => g.Versions).Include(g => g.Scores).FirstOrDefaultAsync(g => g.Id == id);

    public async Task AddAsync(Game g, GameVersion v)
    {
        g.Versions.Add(v);
        db.Games.Add(g);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var g = new Game { Id = id };
        db.Attach(g);
        db.Remove(g);
        await db.SaveChangesAsync();
    }

    public async Task AddScoreAsync(Guid gameId, Score s)
    {
        var game = await db.Games.FindAsync(gameId);
        if (game is null) return;
        game.Scores.Add(s);
        await db.SaveChangesAsync();
    }
}