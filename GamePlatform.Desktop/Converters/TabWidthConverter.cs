using System;
using System.Globalization;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>
///     Oblicza szerokość paska kart tak, by:
///     • zmieścić wszystkie karty przy zachowaniu minimalnej szerokości,
///     • zostawić miejsce na panel nawigacyjny (flagi + Wyloguj),
///     • gdy okno jest zbyt wąskie – pozwala ScrollViewer-owi przewijać.
/// </summary>
public class TabWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType,
                          object parameter, CultureInfo culture)
    {
        // Pobranie danych wejściowych z kontrolą bezpieczeństwa.
        double viewport = values.Length > 0 && values[0] is double v ? v : 0.0;
        int tabCount = values.Length > 1 && values[1] is int c ? c : System.Convert.ToInt32(values[1]);
        double navPanelWidth = values.Length > 2 && values[2] is double nav && !double.IsNaN(nav)
                                                           ? nav
                                                           : 150.0;                     
        double minTabWidth = parameter != null ? System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture)
                                                : 60.0;

        double desired = tabCount * minTabWidth;

        double available = Math.Max(0, viewport - navPanelWidth);
      
        return Math.Max(desired, available);
    }

    public object[] ConvertBack(object value, Type[] targetTypes,
                                object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
