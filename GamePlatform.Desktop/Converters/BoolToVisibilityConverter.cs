using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts a boolean value to Visibility.</summary>
public sealed class BoolToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; }

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool val = value is bool b && b;
        if (Invert) val = !val;
        return val ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}