using System;
using System.Globalization;
using System.Windows.Data;
using GamePlatform.Shared.Enums;

namespace GamePlatform.Desktop.Converters;

public sealed class ReportStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ReportStatus status)
        {
            return status switch
            {
                ReportStatus.Pending => "Oczekuje",
                ReportStatus.InReview => "W trakcie",
                ReportStatus.Closed => "Zamknięte",
                _ => status.ToString()
            };
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}