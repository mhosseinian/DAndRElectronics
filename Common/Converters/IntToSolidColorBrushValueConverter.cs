using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Common.Converters
{
    public class IntToSolidColorBrushValueConverter : IValueConverter
    {
        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
            {
                return null;
            }

            if (Reverse)
            {
                return ConvertBack(value, targetType, parameter, culture);
            }

            var intValue = System.Convert.ToInt32(value);
            var values = BitConverter.GetBytes(intValue);
            if (!BitConverter.IsLittleEndian) Array.Reverse(values);
            var b = values[0];
            var g = values[1];
            var r = values[2];
            var color = Color.FromRgb(r, g, b);
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // If necessary, here you can convert back. Check if which brush it is (if its one),
            // get its Color-value and return it.
            if (value is SolidColorBrush brush)
            {
                var color = brush.Color;
                return color.R << 16 | color.G << 8 | color.B;
            }

            return null;
        }
    }
}