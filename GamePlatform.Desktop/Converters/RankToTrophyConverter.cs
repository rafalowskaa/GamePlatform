using System;
using System.Globalization;
using System.Windows.Data;
using GamePlatform.Shared.Enums;
using System.Windows.Media.Imaging;

namespace GamePlatform.Desktop.Converters;

public sealed class RankToTrophyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not UserRank rank)
            return null;
        string name = rank.ToString();
        string uri = $"pack://application:,,,/Resources/Trophies/{GetBaseName(name)}.png";
        try
        {
            return new BitmapImage(new Uri(uri));
        }
        catch
        {
            return null;
        }
    }

    private static string GetBaseName(string enumName)
    {
        if (enumName.StartsWith("Wood")) return "Wood";
        if (enumName.StartsWith("Bronze")) return "Bronze";
        if (enumName.StartsWith("Silver")) return "Silver";
        if (enumName.StartsWith("Gold")) return "Gold";
        if (enumName.StartsWith("Emerald")) return "Emerald";
        if (enumName.StartsWith("Platinum")) return "Platinum";
        if (enumName.StartsWith("Diamond")) return "Diamond";
        return "Master";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}