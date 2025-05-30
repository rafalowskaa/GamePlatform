using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GamePlatform.Desktop.Converters;

public sealed class AvatarPathToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string path && !string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            try
            {
                var abs = Path.GetFullPath(path);
                return new BitmapImage(new Uri(abs));
            }
            catch { }
        }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}