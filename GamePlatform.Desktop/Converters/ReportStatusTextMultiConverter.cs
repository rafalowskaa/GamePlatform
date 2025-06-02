using GamePlatform.Shared.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using Loc = GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.Converters;

/// <summary>Converts report status + notes to display text.</summary>
public sealed class ReportStatusTextMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length >= 2 && values[0] is ReportStatus status)
        {
            string? notes = values[1] as string;
            if (notes == "Blocked") return Loc.TRaw("STATUS_BLOCKED");
            if (notes == "Cancelled") return Loc.TRaw("STATUS_CANCELLED");

            return status switch
            {
                ReportStatus.Pending => Loc.TRaw("STATUS_NO_ACTION"),
                ReportStatus.Warned => Loc.TRaw("STATUS_WARNING_SENT"),
                ReportStatus.Closed => Loc.TRaw("STATUS_CANCELLED"),
                ReportStatus.InReview => Loc.TRaw("STATUS_IN_REVIEW"),
                _ => status.ToString()
            };
        }
        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}