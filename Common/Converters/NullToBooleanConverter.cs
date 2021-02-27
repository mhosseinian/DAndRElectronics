using System;
using System.Windows.Data;
using System.Collections;

namespace Common.Converters
{
    public class NullToBooleanConverter : IValueConverter
    {
        public bool Reverse
        {
            get;
            set;
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = ((value == null) || string.IsNullOrWhiteSpace(value.ToString()));

            ICollection collectionValue = value as ICollection;

            if (collectionValue != null)
            {
                result = collectionValue.Count < 1;  // empty
            }

            if (Reverse)
            {
                return !result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}