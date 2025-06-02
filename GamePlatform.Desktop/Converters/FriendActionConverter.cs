using System;
using System.Globalization;
using System.Windows.Data;
using GamePlatform.Desktop.Localization;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts a boolean friend flag to menu header text.</summary>
public sealed class FriendActionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isFriend = value is bool b && b;
        return isFriend ? Localization.TRaw("REMOVE_FRIEND") : Localization.TRaw("ADD_FRIEND");
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}