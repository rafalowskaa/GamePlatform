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

    public async Task<bool> LoginAsync(string loginOrEmail, string password)
    {
        _current = await _db.Users.FirstOrDefaultAsync(u =>
                       u.Login == loginOrEmail || u.Email == loginOrEmail);

        if (_current is null) return false;

        /* blokada */
        if (_current.BlockedUntilUtc is not null && _current.BlockedUntilUtc > DateTime.UtcNow)
        {
            Debug.WriteLine($"[Auth] Konto zablokowane do {_current.BlockedUntilUtc}");
            return false;
        }

        bool ok = _hasher.VerifyHashedPassword(_current, _current.PasswordHash, password)
                 != PasswordVerificationResult.Failed;

        if (ok)
        {
            _current.LastLoginUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        else _current = null;

        return ok;
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
            PasswordHash = _hasher.HashPassword(null!, password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task LogoutAsync() { _current = null; return Task.CompletedTask; }
}
