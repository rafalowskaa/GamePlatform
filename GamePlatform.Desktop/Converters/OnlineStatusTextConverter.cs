using System;
using System.Globalization;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

public sealed class OnlineStatusTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isOnline = value is bool b && b;
        return isOnline ? "dostępny" : "niedostępny";
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}