using System;
using System.Globalization;
using System.Windows.Data;

namespace GamePlatform.Desktop.Converters;

/// <summary>Returns sender login depending on whether the message was sent by current user.</summary>
public sealed class MessageSenderConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length >= 4 &&
            values[0] is Guid fromId &&
            values[1] is Guid meId &&
            values[2] is string meLogin &&
            values[3] is string mateLogin)
        {
            return fromId == meId ? meLogin : mateLogin;
        }
        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}