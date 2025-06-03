namespace GamePlatform.Shared.Enums;

public static class RankHelper
{
    public static int GetNextXp(int xp)
    {
        if (xp < 1000) return 1000;
        if (xp < 3000) return 3000;
        if (xp < 6000) return 6000;
        if (xp < 10000) return 10000;
        return xp;
    }
}