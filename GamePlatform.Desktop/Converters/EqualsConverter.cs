using System;
using System.Globalization;
using System.Windows.Data;
namespace GamePlatform.Desktop.Converters;
public sealed class EqualsConverter : IMultiValueConverter
{
    public object Convert(object[] v, Type t, object p, CultureInfo c)
        => v.Length == 2 && Equals(v[0], v[1]);
    public object[] ConvertBack(object v, Type[] t, object p, CultureInfo c)
        => throw new NotSupportedException();
}