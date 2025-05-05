// Services/AuthService.cs
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();
    private User? _current;

    public AuthService(AppDbContext db) => _db = db;

    public UserRole CurrentRole => _current?.Role ?? UserRole.Guest;
    public string? CurrentLogin => _current?.Login;

    /* ----------------  LOGIN  ---------------- */
    public async Task<bool> LoginAsync(string loginOrEmail, string password)
    {
        loginOrEmail = loginOrEmail.Trim();

        _current = await _db.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u =>
                                 u.Login == loginOrEmail ||
                                 u.Email == loginOrEmail);

        if (_current is null) return false;

        var ok = _hasher.VerifyHashedPassword(_current,
                                              _current.PasswordHash,
                                              password) != PasswordVerificationResult.Failed;

        if (!ok) _current = null;
        return ok;
    }

    /* --------------  REJESTRACJA  -------------- */
    public async Task<bool> RegisterAsync(string login, string email, string password)
    {
        login = login.Trim();
        email = email.Trim();

        if (await _db.Users.AnyAsync(u => u.Login == login || u.Email == email))
            return false;

        var user = new User
        {
            Login = login,
            Email = email,
            Role = UserRole.User
        };
        user.PasswordHash = _hasher.HashPassword(user, password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _current = user;                 // auto-login po rejestracji
        return true;
    }

    public Task LogoutAsync()
    {
        _current = null;
        return Task.CompletedTask;
    }
}
