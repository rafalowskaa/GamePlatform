using GamePlatform.Shared.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts report status + notes to a brush color.</summary>
public sealed class ReportStatusBrushMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length >= 2 && values[0] is ReportStatus status)
        {
            string? notes = values[1] as string;
            if (notes == "Blocked") return Brushes.Red;
            if (notes == "Cancelled") return Brushes.LimeGreen;

            return status switch
            {
                ReportStatus.Warned => Brushes.Orange,
                ReportStatus.Pending => Brushes.White,
                ReportStatus.Closed => Brushes.LimeGreen,
                _ => Brushes.White
            };
        }
        return Brushes.White;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}