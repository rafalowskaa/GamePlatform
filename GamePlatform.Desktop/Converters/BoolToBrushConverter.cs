using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts a boolean value to one of two brushes.</summary>
public sealed class BoolToBrushConverter : IValueConverter
{
    public Brush TrueBrush { get; set; } = Brushes.DodgerBlue;
    public Brush FalseBrush { get; set; } = Brushes.DarkSlateGray;

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && b ? TrueBrush : FalseBrush;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}