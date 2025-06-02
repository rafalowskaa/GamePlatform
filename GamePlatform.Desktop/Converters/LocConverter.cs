using System;
using System.Globalization;
using System.Windows.Data;
using GamePlatform.Desktop.Localization;

namespace GamePlatform.Desktop.Converters;

public sealed class LocConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => Localization.T((string)parameter!);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;
}