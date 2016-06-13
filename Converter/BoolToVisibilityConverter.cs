using System;
using System.Globalization;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OpenUWP.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                bool isVisibility = (bool)value;

                string param = (string)parameter;

                if (!String.IsNullOrEmpty(param))
                {
                    return isVisibility ? Visibility.Collapsed : Visibility.Visible;
                }
                return isVisibility ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool isVisibility = (bool)value;

                string param = (string) parameter;

                if (!String.IsNullOrEmpty(param))
                {
                    return isVisibility ? Visibility.Collapsed : Visibility.Visible;
                }
                return isVisibility ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
