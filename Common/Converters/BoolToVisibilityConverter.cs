using System;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
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
            Visibility visibility = Hide ? Visibility.Hidden : Visibility.Collapsed;

            if (value is bool)
            {
                if (_reverse)
                {
                    return (bool)value ? visibility : Visibility.Visible;
                }
                else
                {
                    return (bool)value ? Visibility.Visible : visibility;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
            {
                if (_reverse)
                {
                    return (Visibility)value == Visibility.Visible ? false : true;
                }
                else
                {
                    return (Visibility)value == Visibility.Visible ? true : false;
                }
            }
            return false;
        }

        #endregion
    }
}