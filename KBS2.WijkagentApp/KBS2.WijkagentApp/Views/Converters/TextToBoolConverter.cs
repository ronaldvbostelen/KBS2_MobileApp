using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Views.Converters
{
    class TextToBoolConverter : IValueConverter
    {
        //simple converter for string to bool
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return !string.IsNullOrEmpty((string)value);
        }
        
        //only need oneway convert, so didnt implement this
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
