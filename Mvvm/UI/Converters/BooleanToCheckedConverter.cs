using System;
using System.Globalization;
using System.Windows.Data;

namespace LisaTrigger.Mvvm.UI.Converters;

public class BooleanToCheckedConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;


        string? checkValue = value.ToString();
        var targetValue = parameter.ToString();
        return checkValue != null && checkValue.Equals(targetValue,
            StringComparison.InvariantCultureIgnoreCase);
    }

    public object? ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return null;

        string? checkValue = value.ToString();
        string? targetValue = parameter.ToString();
        return checkValue != null && checkValue.Equals(targetValue,
            StringComparison.InvariantCultureIgnoreCase);
    }
}