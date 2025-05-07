using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>
/// bool → Visibility  (parametr "Invert" neguje wynik)
/// </summary>
public sealed class BoolToVisConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
                          object parameter, CultureInfo culture)
    {
        bool visible = value is true;

        if (parameter is string s &&
            s.Equals("Invert", StringComparison.OrdinalIgnoreCase))
            visible = !visible;

        return visible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object v, Type t, object p, CultureInfo c)
        => throw new NotSupportedException();
}
