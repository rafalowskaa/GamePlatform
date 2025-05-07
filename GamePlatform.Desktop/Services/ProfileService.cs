using GamePlatform.Shared.Data;
using GamePlatform.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

/// <inheritdoc />
public sealed class ProfileService : IProfileService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();

    public ProfileService(AppDbContext db) => _db = db;

    public async Task<bool> UpdateAsync(User updated, string? newPwd, Stream? avatar)
    {
        var user = await _db.Users.FirstAsync(u => u.Id == updated.Id);

        /* podstawowe dane */
        user.Login = updated.Login;
        user.Email = updated.Email;
        user.RecoveryMail = updated.RecoveryMail;

        /* zmiana hasła */
        if (!string.IsNullOrWhiteSpace(newPwd))
            user.PasswordHash = _hasher.HashPassword(user, newPwd);

        /* avatar */
        if (avatar is not null)
        {
            Directory.CreateDirectory("Avatars");
            var path = Path.Combine("Avatars", $"{user.Id}.png");
            user.AvatarPath = path;

            using var fs = File.Create(path);
            await avatar.CopyToAsync(fs);
        }

        await _db.SaveChangesAsync();
        return true;
    }
}
