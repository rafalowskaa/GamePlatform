using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Models;
using GamePlatform.Desktop.Services;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Desktop.Services;

/// <inheritdoc />
public sealed class GameRepositorySqlite : IGameRepository
{
    private readonly AppDbContext _db;
    public GameRepositorySqlite(AppDbContext db) => _db = db;

    private static void SetCover(Game g)
    {
        var folder = g.Versions.OrderByDescending(v => v.Number).FirstOrDefault()?.FolderPath;
        if (!string.IsNullOrEmpty(folder))
        {
            foreach (var name in new[] { "cover.jpg", "cover.png" })
            {
                var path = Path.Combine(folder, name);
                if (File.Exists(path)) { g.CoverPath = path; break; }
            }
        }
    }

    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);
        foreach (var file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
        {
            string rel = Path.GetRelativePath(sourceDir, file);
            string dest = Path.Combine(destDir, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
            File.Copy(file, dest, true);
        }
    }

    /* ------------ QUERY ------------ */

    public async Task<IReadOnlyList<Game>> GetAllAsync(string? filter = null)
    {
        IQueryable<Game> q = _db.Games.Include(g => g.Versions)
                                      .Include(g => g.SubmittedByUser)
                                      .Where(g => g.IsApproved)
                                      .AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filter))
            q = q.Where(g => g.Title.Contains(filter));
        var list = await q.ToListAsync();
        foreach (var g in list) SetCover(g);
        return list;
    }

    public async Task<Game?> GetAsync(Guid id)
    {
        var g = await _db.Games
                         .Include(g => g.Versions)
                         .Include(g => g.Scores)
                         .Include(g => g.SubmittedByUser)
                         .FirstOrDefaultAsync(g => g.Id == id);
        if (g is not null) SetCover(g);
        return g;
    }

    /* ------------ COMMANDS ------------ */

    public async Task AddAsync(Game game, GameVersion firstVersion)
    {
        /* katalog docelowy */
        string root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GamePlatform", "Games", game.Id.ToString());
        Directory.CreateDirectory(root);

        string ext = Path.GetExtension(firstVersion.FolderPath).ToLowerInvariant();
        string extractDir = Path.Combine(root, firstVersion.Number);
        if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);


        if (ext == ".zip")
        {
            string zipDest = Path.Combine(root, $"{firstVersion.Number}.zip");
            File.Copy(firstVersion.FolderPath, zipDest, overwrite: true);
            ZipFile.ExtractToDirectory(zipDest, extractDir);
        }
        else if (ext == ".exe")
        {
            string sourceDir = Path.GetDirectoryName(firstVersion.FolderPath)!;
            CopyDirectory(sourceDir, extractDir);
        }

        if (!string.IsNullOrEmpty(game.CoverPath) && File.Exists(game.CoverPath))
        {
            string extImg = Path.GetExtension(game.CoverPath).ToLowerInvariant();
            if (extImg is not (".jpg" or ".png")) extImg = ".jpg";
            File.Copy(game.CoverPath, Path.Combine(extractDir, $"cover{extImg}"), true);
        }

        firstVersion.FolderPath = extractDir;
        game.Versions.Add(firstVersion);

        _db.Games.Add(game);
        await _db.SaveChangesAsync();
         SetCover(game);
    }

    public async Task UpdateAsync(Game game)
    {
        _db.Games.Update(game);
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Game>> GetPendingAsync()
    {
        var list = await _db.Games.Include(g => g.Versions)
                                  .Include(g => g.SubmittedByUser)
                                  .Where(g => !g.IsApproved)
                                  .AsNoTracking()
                                  .ToListAsync();
        foreach (var g in list) SetCover(g);
        return list;
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

    public async Task ToggleFavoriteAsync(Guid userId, Guid gameId)
    {
        var fav = await _db.FavoriteGames.FindAsync(userId, gameId);
        if (fav is null)
            _db.FavoriteGames.Add(new FavoriteGame { UserId = userId, GameId = gameId });
        else
            _db.FavoriteGames.Remove(fav);
        await _db.SaveChangesAsync();
    }

    public async Task AddPlayTimeAsync(Guid userId, Guid gameId, int seconds)
    {
        var fav = await _db.FavoriteGames.FindAsync(userId, gameId);
        if (fav is null)
        {
            fav = new FavoriteGame
            {
                UserId = userId,
                GameId = gameId,
                PlaytimeSeconds = seconds,
                LastPlayedUtc = DateTime.UtcNow
            };
            _db.FavoriteGames.Add(fav);
        }
        else
        {
            fav.PlaytimeSeconds += seconds;
            fav.LastPlayedUtc = DateTime.UtcNow;
        }
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Game>> GetFavoritesAsync(Guid userId)
    {
        var login = await _db.Users
                             .Where(u => u.Id == userId)
                             .Select(u => u.Login)
                             .FirstAsync();

        var list = await _db.FavoriteGames
                            .Where(f => f.UserId == userId)
                            .Include(f => f.Game!)
                                .ThenInclude(g => g.Versions)
                            .AsNoTracking()
                            .ToListAsync();
        var result = new List<Game>();
        foreach (var fav in list)
        {
            var g = fav.Game!;
            SetCover(g);
            g.LastPlayed = fav.LastPlayedUtc;
            g.PlaytimeSeconds = fav.PlaytimeSeconds;

            var scores = await _db.Scores
                                   .Where(s => s.GameId == g.Id && s.Player == login)
                                   .ToListAsync();
            if (scores.Count > 0)
            {
                g.LastPlayed = scores.Max(s => (DateTime?)s.CreatedUtc);
                g.HighestScore = scores.Max(s => s.Value);
            }
            result.Add(g);
        }
        return result;
    }
}
