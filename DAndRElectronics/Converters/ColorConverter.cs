using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DAndRElectronics.Enums;

namespace DAndRElectronics.Converters
{
    public class ColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SupportedColors status)
            {

                switch (status)
                {
                    case SupportedColors.Black:
                        return  new SolidColorBrush(Colors.Black);
                    case SupportedColors.Red:
                        return new SolidColorBrush(Colors.Red);
                    case SupportedColors.Green:
                        return new SolidColorBrush(Colors.Green);
                    case SupportedColors.Blue:
                        return new SolidColorBrush(Colors.Blue);
                    case SupportedColors.Yellow:
                        return new SolidColorBrush(Colors.Yellow);
                    case SupportedColors.White:
                        return new SolidColorBrush(Colors.White);
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}