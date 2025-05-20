using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Desktop.Services;

/// <inheritdoc />
public sealed class GameRepositorySqlite : IGameRepository
{
    private readonly AppDbContext _db;
    public GameRepositorySqlite(AppDbContext db) => _db = db;

    /* ------------ QUERY ------------ */

    public async Task<IReadOnlyList<Game>> GetAllAsync(string? filter = null)
    {
        IQueryable<Game> q = _db.Games.Include(g => g.Versions).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filter))
            q = q.Where(g => g.Title.Contains(filter));
        return await q.ToListAsync();
    }

    public async Task<Game?> GetAsync(Guid id) =>
        await _db.Games
                 .Include(g => g.Versions)
                 .Include(g => g.Scores)
                 .FirstOrDefaultAsync(g => g.Id == id);

    /* ------------ COMMANDS ------------ */

    public async Task AddAsync(Game game, GameVersion firstVersion)
    {
        /* katalog docelowy */
        string root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GamePlatform", "Games", game.Id.ToString());
        Directory.CreateDirectory(root);

        /* kopiujemy ZIP */
        string zipDest = Path.Combine(root, $"{firstVersion.Number}.zip");
        File.Copy(firstVersion.FolderPath, zipDest, overwrite: true);

        /* rozpakowujemy */
        string extractDir = Path.Combine(root, firstVersion.Number);
        if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
        ZipFile.ExtractToDirectory(zipDest, extractDir);

        firstVersion.FolderPath = extractDir;
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
        var g = await _db.Games.FindAsync(gameId);
        if (g is null) return;
        g.Scores.Add(score);
        await _db.SaveChangesAsync();
    }

    /* ------------ LEADERBOARD ------------ */

    public async Task<IReadOnlyList<Score>> GetTopScoresAsync(Guid gameId, int limit) =>
        await _db.Scores.Where(s => s.GameId == gameId)
                        .OrderByDescending(s => s.Value)
                        .Take(limit)
                        .AsNoTracking()
                        .ToListAsync();
}
