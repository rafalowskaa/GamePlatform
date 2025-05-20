using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace GamePlatform.Desktop.Converters;
public sealed class BoolToVisConverter : IValueConverter
{
    public object Convert(object v, Type t, object p, CultureInfo c)
    {
        bool visible = v is true;
        if (p is string s && s.Equals("Invert", StringComparison.OrdinalIgnoreCase))
            visible = !visible;
        return visible ? Visibility.Visible : Visibility.Collapsed;
    }
    public object ConvertBack(object v, Type t, object p, CultureInfo c)
        => throw new NotSupportedException();
}