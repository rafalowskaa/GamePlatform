using Microsoft.EntityFrameworkCore;
using GamePlatform.Shared.Models;

namespace GamePlatform.Shared.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public AppDbContext() { }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameVersion> GameVersions => Set<GameVersion>();
    public DbSet<Score> Scores => Set<Score>();

    protected override void OnConfiguring(DbContextOptionsBuilder b)
    {
        if (!b.IsConfigured)
            b.UseSqlite("Data Source=games.db");
    }
}