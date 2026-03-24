namespace ModernWPF.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
            return intValue != 0 ? Visibility.Visible : Visibility.Hidden;

        if (value is string strValue)
            return strValue.Equals("true", StringComparison.OrdinalIgnoreCase) ? Visibility.Visible : Visibility.Hidden;

        if (value is bool boolValue)
            return boolValue ? Visibility.Visible : Visibility.Hidden;

        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}