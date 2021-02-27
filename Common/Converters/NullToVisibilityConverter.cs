using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {

        private bool _reverse;

        /// <summary>
        /// Setting Reverse to True will return hidden for a true value
        /// </summary>
        public bool Reverse
        {
            get
            {
                return _reverse;
            }
            set
            {
                _reverse = value;
            }
        }

        public bool Hide { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            NullToBooleanConverter bconverter = new NullToBooleanConverter();
            bool bValue = (bool)bconverter.Convert(value, targetType, parameter, culture);

            Visibility visibility = Hide ? Visibility.Hidden : Visibility.Collapsed;

            if (bValue)
            {
                return _reverse ? visibility : Visibility.Visible;
            }
            else
            {
                return _reverse ? Visibility.Visible : visibility;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //don't know how to convert back
            throw new NotImplementedException();
        }

        #endregion    
    }
}