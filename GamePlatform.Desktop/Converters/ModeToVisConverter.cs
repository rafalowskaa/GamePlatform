using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GamePlatform.Shared.Enums;

namespace GamePlatform.Desktop.Converters
{
    /// <summary>Visible tylko dla trybu MultiPlayer</summary>
    public sealed class ModeToVisConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c) =>
            value is GameMode m && m == GameMode.MultiPlayer
                ? Visibility.Visible
                : Visibility.Collapsed;

        public object ConvertBack(object value, Type t, object p, CultureInfo c) =>
            throw new NotSupportedException();
    }
}
