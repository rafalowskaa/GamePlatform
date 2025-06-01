using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();
    private User? _current;

    public AuthService(AppDbContext db) => _db = db;

    public User? CurrentUser => _current;
    public UserRole CurrentRole => _current?.Role ?? UserRole.Guest;
    public string? CurrentLogin => _current?.Login;

    public async Task<LoginResult> LoginAsync(string loginOrEmail, string password)
    {
        _current = await _db.Users.FirstOrDefaultAsync(u =>
                       u.Login == loginOrEmail || u.Email == loginOrEmail);

        if (_current is null)
            return LoginResult.Fail("Błędny login lub hasło.");

        /* sprawdź blokadę */
        if (_current.BlockedUntilUtc is not null)
        {
            if (_current.BlockedUntilUtc > DateTime.UtcNow)
            {
                string msg = $"Konto zostało zablokowane do {_current.BlockedUntilUtc:yyyy-MM-dd HH:mm} (UTC)"
                           + (!string.IsNullOrWhiteSpace(_current.BlockReason)
                              ? $"\nPowód: {_current.BlockReason}"
                              : "");
                return LoginResult.Fail(msg);
            }
            else
            {
                _current.BlockedUntilUtc = null;
                _current.BlockReason = null;
                await _db.SaveChangesAsync();
            }
        }

        bool ok = _hasher.VerifyHashedPassword(_current, _current.PasswordHash, password)
                 != PasswordVerificationResult.Failed;

        if (ok)
        {
            _current.LastLoginUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return LoginResult.Success();
        }

        _current = null;
        return LoginResult.Fail("Błędny login lub hasło.");
    }

    public async Task<bool> RegisterAsync(string login, string email, string password)
    {
        if (await _db.Users.AnyAsync(u => u.Login == login || u.Email == email))
            return false;

        var user = new User
        {
            Login = login,
            Email = email,
            Role = UserRole.User,
            PasswordHash = _hasher.HashPassword(null!, password),
            Xp = 0,
            Rank = UserRank.Bronze
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task LogoutAsync() { _current = null; return Task.CompletedTask; }
}

/* ——— pomocniczy DTO wyniku logowania ——— */
public record LoginResult(bool IsSuccess, string? Error)
{
    public static LoginResult Success() => new(true, null);
    public static LoginResult Fail(string reason) => new(false, reason);
}
