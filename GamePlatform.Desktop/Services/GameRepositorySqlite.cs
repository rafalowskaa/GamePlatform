using GamePlatform.Shared.Data;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services
{
    public class GameRepositorySqlite : IGameRepository
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
            var stub = new Game { Id = id };
            _db.Attach(stub);
            _db.Remove(stub);
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
}
