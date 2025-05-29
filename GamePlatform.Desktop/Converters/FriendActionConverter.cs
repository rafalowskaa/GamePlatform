using System;
using System.Globalization;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts a boolean friend flag to menu header text.</summary>
public sealed class FriendActionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isFriend = value is bool b && b;
        return isFriend ? "Usuń ze znajomych" : "Dodaj do znajomych";
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}