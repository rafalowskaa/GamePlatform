using FluentValidation;
using FluentValidation.Results;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace GamePlatform.Shared.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }
    public AppDbContext() { }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameVersion> GameVersions => Set<GameVersion>();
    public DbSet<Score> Scores => Set<Score>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder b)
    {
        if (!b.IsConfigured) b.UseSqlite("Data Source=games.db");
    }

    // ------------  walidacja FluentValidation  ------------
    public override int SaveChanges()
    {
        ValidateEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        ValidateEntities();
        return await base.SaveChangesAsync(ct);
    }

    private void ValidateEntities()
    {
        var sp = this.GetService<IServiceProvider>();

        foreach (var e in ChangeTracker.Entries()
                                       .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            var valType = typeof(IValidator<>).MakeGenericType(e.Entity.GetType());
            var validator = sp?.GetService(valType) as IValidator;
            if (validator is null) continue;

            // -------------  POPRAWKA  -------------
            var ctx = new ValidationContext<object>(e.Entity);
            var res = validator.Validate(ctx);
            // --------------------------------------
            if (!res.IsValid)
                throw new ValidationException(res.Errors);
        }
    }

}
