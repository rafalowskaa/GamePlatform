using System;
using System.Globalization;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

public class TabWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType,
                          object parameter, CultureInfo culture)
    {
        double viewport = (double)values[0];
        int count = System.Convert.ToInt32(values[1]);
        double minTab = System.Convert.ToDouble(parameter);

        double desired = count * minTab;
        double available = Math.Max(0, viewport - 170);

        return Math.Max(desired, available);
    }

    public object[] ConvertBack(object value, Type[] targetTypes,
                                object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
