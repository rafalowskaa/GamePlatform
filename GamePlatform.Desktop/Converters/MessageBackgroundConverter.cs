using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GamePlatform.Desktop.Converters;

/// <summary>Returns a Brush depending on whether the message was sent by the current user.</summary>
public sealed class MessageBackgroundConverter : IMultiValueConverter
{
    public Brush FromMeBrush { get; set; } = Brushes.DodgerBlue;
    public Brush FromMateBrush { get; set; } = Brushes.DarkSlateGray;

    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length >= 2 && values[0] is Guid fromId && values[1] is Guid meId)
            return fromId == meId ? FromMeBrush : FromMateBrush;
        return FromMateBrush;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}