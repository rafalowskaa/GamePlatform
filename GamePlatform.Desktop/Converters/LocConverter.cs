using System;
using System.Globalization;
using System.Windows.Data;
using Loc = GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.Converters;

public sealed class LocConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => Loc.T((string)parameter!);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;
}