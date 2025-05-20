using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GamePlatform.Shared.Enums;
namespace GamePlatform.Desktop.Converters;
public sealed class ModeToVisConverter : IValueConverter
{
    public object Convert(object v, Type t, object p, CultureInfo c)
        => v is GameMode m && m == GameMode.MultiPlayer
           ? Visibility.Visible : Visibility.Collapsed;
    public object ConvertBack(object v, Type t, object p, CultureInfo c)
        => throw new NotSupportedException();
}