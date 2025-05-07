using FluentValidation;
using FluentValidation.Results;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamePlatform.Shared.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }
    public AppDbContext() { }

    /* ---------- DbSet-y ---------- */
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameVersion> GameVersions => Set<GameVersion>();
    public DbSet<Score> Scores => Set<Score>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Friend> Friends => Set<Friend>();
    public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();

    /* ---------- konfiguracja ---------- */
    protected override void OnConfiguring(DbContextOptionsBuilder b)
    {
        if (!b.IsConfigured)
            b.UseSqlite("Data Source=games.db");
    }

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        /* ===== tabela Friend – relacja N:N z dodatkowymi kolumnami ===== */
        b.Entity<Friend>(cfg =>
        {
            /* klucz złożony zapobiega duplikatom (User ↔ Mate) */
            cfg.HasKey(f => new { f.UserId, f.MateId });

            cfg.HasOne(f => f.User)          // zapraszający
               .WithMany(u => u.FriendsInitiated)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            cfg.HasOne(f => f.Mate)          // przyjęty
               .WithMany(u => u.FriendsAccepted)
               .HasForeignKey(f => f.MateId)
               .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /* ---------- walidacja (FluentValidation) ---------- */
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
            var validatorType = typeof(IValidator<>).MakeGenericType(e.Entity.GetType());
            if (sp?.GetService(validatorType) is not IValidator validator) continue;

            var ctx = new ValidationContext<object>(e.Entity);
            ValidationResult res = validator.Validate(ctx);

            if (!res.IsValid)
                throw new ValidationException(res.Errors);
        }
    }
}
