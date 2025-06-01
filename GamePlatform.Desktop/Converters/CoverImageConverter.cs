using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GamePlatform.Desktop.Converters;

public sealed class CoverImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string folder && Directory.Exists(folder))
        {
            string[] names = ["cover.jpg", "cover.png"];
            foreach (var name in names)
            {
                var path = Path.Combine(folder, name);
                if (File.Exists(path))
                {
                    try { return new BitmapImage(new Uri(Path.GetFullPath(path))); } catch { }
                }
            }
        }
        return new BitmapImage(new Uri("pack://application:,,,/Resources/login_banner.png"));
    }

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}