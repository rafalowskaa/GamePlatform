using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts a boolean to horizontal alignment.</summary>
public sealed class BoolToHAlignmentConverter : IValueConverter
{
    public HorizontalAlignment TrueAlignment { get; set; } = HorizontalAlignment.Right;
    public HorizontalAlignment FalseAlignment { get; set; } = HorizontalAlignment.Left;

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b ? TrueAlignment : FalseAlignment;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}