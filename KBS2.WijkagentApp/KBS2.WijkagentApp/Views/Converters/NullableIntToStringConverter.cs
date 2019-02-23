using System;
using System.Globalization;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Views.Converters
{
    class NullableIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((int?) value).ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int.TryParse((string)value, out var result);
            return result;
        } 
         
    }
}
