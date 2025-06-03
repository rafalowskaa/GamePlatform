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

    private static readonly (int xp, UserRank rank)[] _thresholds =
    [
        (0, UserRank.WoodV),
        (200, UserRank.WoodIV),
        (400, UserRank.WoodIII),
        (600, UserRank.WoodII),
        (800, UserRank.WoodI),

        (1000, UserRank.BronzeV),
        (1200, UserRank.BronzeIV),
        (1400, UserRank.BronzeIII),
        (1600, UserRank.BronzeII),
        (1800, UserRank.BronzeI),

        (2000, UserRank.SilverV),
        (2200, UserRank.SilverIV),
        (2400, UserRank.SilverIII),
        (2600, UserRank.SilverII),
        (2800, UserRank.SilverI),

        (3000, UserRank.GoldV),
        (3200, UserRank.GoldIV),
        (3400, UserRank.GoldIII),
        (3600, UserRank.GoldII),
        (3800, UserRank.GoldI),

        (4000, UserRank.EmeraldV),
        (4200, UserRank.EmeraldIV),
        (4400, UserRank.EmeraldIII),
        (4600, UserRank.EmeraldII),
        (4800, UserRank.EmeraldI),

        (5000, UserRank.PlatinumV),
        (5200, UserRank.PlatinumIV),
        (5400, UserRank.PlatinumIII),
        (5600, UserRank.PlatinumII),
        (5800, UserRank.PlatinumI),

        (6000, UserRank.DiamondV),
        (6200, UserRank.DiamondIV),
        (6400, UserRank.DiamondIII),
        (6600, UserRank.DiamondII),
        (6800, UserRank.DiamondI),

        (7000, UserRank.Master)
    ];

    private static UserRank CalculateRank(int xp)
    {
        for (int i = _thresholds.Length - 1; i >= 0; i--)
        {
            if (xp >= _thresholds[i].xp)
                return _thresholds[i].rank;
        }
        return UserRank.WoodV;
    }
}