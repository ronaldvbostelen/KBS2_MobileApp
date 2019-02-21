using System;
using System.Globalization;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Views.Converters
{
    class IntToBoolConverter : IValueConverter
    {
        //simple converter for >=0 || null
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int) value >= 0;
        
        //one way therefore not implementing convertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
