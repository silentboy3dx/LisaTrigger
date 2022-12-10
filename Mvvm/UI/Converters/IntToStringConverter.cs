namespace LisaTrigger.Mvvm.UI.Converters;

public class IntToString
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int ret = 0;
        return int.TryParse((string)value, out ret) ? ret : 0;
    }
}