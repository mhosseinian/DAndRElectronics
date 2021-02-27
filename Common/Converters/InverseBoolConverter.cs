using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters
{
    [ValueConversion(typeof(Boolean), typeof(Boolean))]
    public class InverseBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Boolean)
                return !((Boolean)value);
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Boolean)
                return !((Boolean)value);
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}