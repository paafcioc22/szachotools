using System;
using System.Globalization;
using Xamarin.Forms;
namespace LabelTextColorSample
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsString = (bool)value;
            switch (valueAsString)
            {
                case (true):
                    {
                        return Color.Black;
                    }
                case (false):
                    {
                        return Color.Green;
                    }
                //default:
                //    {
                //        return Color.Green;//FromHex(value.ToString());
                //    }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}