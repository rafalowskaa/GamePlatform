using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GamePlatform.Desktop.Converters;

public sealed class OnlineBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isOnline = value is bool b && b;
        return isOnline ? Brushes.LimeGreen : Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}