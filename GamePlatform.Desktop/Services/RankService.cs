using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using System;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public sealed class RankService : IRankService
{
    private readonly AppDbContext _db;
    public RankService(AppDbContext db) => _db = db;

    public async Task AddXpAsync(Guid userId, int xp)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return;

        user.Xp += xp;
        var newRank = CalculateRank(user.Xp);
        if (newRank > user.Rank)
            user.Rank = newRank;

        await _db.SaveChangesAsync();
    }

    private static UserRank CalculateRank(int xp)
    {
        if (xp >= 10000) return UserRank.Diamond;
        if (xp >= 6000) return UserRank.Platinum;
        if (xp >= 3000) return UserRank.Gold;
        if (xp >= 1000) return UserRank.Silver;
        return UserRank.Bronze;
    }
}