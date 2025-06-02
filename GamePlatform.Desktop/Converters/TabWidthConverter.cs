using System;
using System.Globalization;
using System.Windows.Data;

public class TabWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType,
                          object parameter, CultureInfo culture)
    {
        double viewport = (double)values[0];
        int count = System.Convert.ToInt32(values[1]);
        double minTab = System.Convert.ToDouble(parameter);   // 120

        double desired = count * minTab;
        return Math.Max(viewport, desired);     // szerokość UniformGrid
    }

    public object[] ConvertBack(object value, Type[] targetTypes,
                                object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
