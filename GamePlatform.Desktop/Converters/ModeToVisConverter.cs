using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GamePlatform.Shared.Enums;

namespace GamePlatform.Desktop.Converters;

/// <summary>
/// Widoczność tylko wtedy, gdy gra ma tryb MultiPlayer.
/// </summary>
public sealed class ModeToVisConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
                          object parameter, CultureInfo culture)
    {
        return value is GameMode m && m == GameMode.MultiPlayer
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object ConvertBack(object v, Type t, object p, CultureInfo c)
        => throw new NotSupportedException();
}
