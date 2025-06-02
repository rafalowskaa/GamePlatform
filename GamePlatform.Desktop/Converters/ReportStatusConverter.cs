using System;
using System.Globalization;
using System.Windows.Data;
using GamePlatform.Shared.Enums;
using Loc = GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.Converters;

public sealed class ReportStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ReportStatus status)
        {
            return status switch
            {
                ReportStatus.Pending => Loc.TRaw("STATUS_PENDING"),
                ReportStatus.InReview => Loc.TRaw("STATUS_IN_REVIEW"),
                ReportStatus.Closed => Loc.TRaw("STATUS_CLOSED"),
                ReportStatus.Warned => Loc.TRaw("STATUS_WARNED"),
                _ => status.ToString()
            };
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}