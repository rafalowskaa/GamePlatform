namespace GamePlatform.Shared.Enums;

public static class RankHelper
{

    private static readonly int[] _xpThresholds =
   [
       0, 200, 400, 600, 800,
        1000, 1200, 1400, 1600, 1800,
        2000, 2200, 2400, 2600, 2800,
        3000, 3200, 3400, 3600, 3800,
        4000, 4200, 4400, 4600, 4800,
        5000, 5200, 5400, 5600, 5800,
        6000, 6200, 6400, 6600, 6800,
        7000
   ];

    private static readonly (int xp, UserRank rank)[] _thresholds =
    [
        (0, UserRank.WoodV), (200, UserRank.WoodIV), (400, UserRank.WoodIII), (600, UserRank.WoodII), (800, UserRank.WoodI),
        (1000, UserRank.BronzeV), (1200, UserRank.BronzeIV), (1400, UserRank.BronzeIII), (1600, UserRank.BronzeII), (1800, UserRank.BronzeI),
        (2000, UserRank.SilverV), (2200, UserRank.SilverIV), (2400, UserRank.SilverIII), (2600, UserRank.SilverII), (2800, UserRank.SilverI),
        (3000, UserRank.GoldV), (3200, UserRank.GoldIV), (3400, UserRank.GoldIII), (3600, UserRank.GoldII), (3800, UserRank.GoldI),
        (4000, UserRank.EmeraldV), (4200, UserRank.EmeraldIV), (4400, UserRank.EmeraldIII), (4600, UserRank.EmeraldII), (4800, UserRank.EmeraldI),
        (5000, UserRank.PlatinumV), (5200, UserRank.PlatinumIV), (5400, UserRank.PlatinumIII), (5600, UserRank.PlatinumII), (5800, UserRank.PlatinumI),
        (6000, UserRank.DiamondV), (6200, UserRank.DiamondIV), (6400, UserRank.DiamondIII), (6600, UserRank.DiamondII), (6800, UserRank.DiamondI),
        (7000, UserRank.Master)
    ];
    public static int GetNextXp(int xp)
    {
        foreach (var t in _xpThresholds)
            if (xp < t) return t;
        return xp;
    }

    public static UserRank GetRank(int xp)
    {
        UserRank rank = UserRank.WoodV;
        foreach (var t in _thresholds)
            if (xp >= t.xp) rank = t.rank;
        return rank;
    }

    public static UserRank GetNextRank(int xp)
    {
        foreach (var t in _thresholds)
            if (xp < t.xp) return t.rank;
        return UserRank.Master;
    }
}