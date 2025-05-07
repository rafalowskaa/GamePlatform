using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Desktop.Services;

/// <inheritdoc />
public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();
    private User? _current;

    public AuthService(AppDbContext db) => _db = db;

    /* -------------  IAuthService  ---------------- */

    public User? CurrentUser => _current;
    public UserRole CurrentRole => _current?.Role ?? UserRole.Guest;
    public string? CurrentLogin => _current?.Login;

    public async Task<bool> LoginAsync(string loginOrEmail, string password)
    {
        try
        {
            _current = await _db.Users.FirstOrDefaultAsync(u =>
                            u.Login == loginOrEmail || u.Email == loginOrEmail);

            if (_current is null) return false;

            var ok = _hasher.VerifyHashedPassword(_current,
                                                  _current.PasswordHash,
                                                  password) != PasswordVerificationResult.Failed;

            if (!ok) _current = null;
            return ok;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[LoginAsync] {ex}");
            return false;                       // błąd → bez crashu
        }
    }

    public async Task<bool> RegisterAsync(string login, string email, string password)
    {
        try
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

            _current = user;                    // ★ auto-login
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[RegisterAsync] {ex}");
            return false;
        }
    }

    public Task LogoutAsync()
    {
        _current = null;
        return Task.CompletedTask;
    }
}
