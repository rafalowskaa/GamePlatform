using GamePlatform.Shared.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts report status + notes to display text.</summary>
public sealed class ReportStatusTextMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length >= 2 && values[0] is ReportStatus status)
        {
            string? notes = values[1] as string;
            if (notes == "Blocked") return "Zablokowano";
            if (notes == "Cancelled") return "Anulowano";

            return status switch
            {
                ReportStatus.Pending => "Niepodjęto akcji",
                ReportStatus.Warned => "Wysłano ostrzeżenie",
                ReportStatus.Closed => "Anulowano",
                ReportStatus.InReview => "W trakcie",
                _ => status.ToString()
            };
        }
        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}