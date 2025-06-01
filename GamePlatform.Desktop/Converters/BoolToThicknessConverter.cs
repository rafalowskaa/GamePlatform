using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts a boolean value to one of two Thickness values.</summary>
public sealed class BoolToThicknessConverter : IValueConverter
{
    public Thickness TrueThickness { get; set; } = new Thickness(0);
    public Thickness FalseThickness { get; set; } = new Thickness(0, 2, 0, 2);

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b ? TrueThickness : FalseThickness;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}