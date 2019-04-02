using System;
using System.Globalization;
using Xamarin.Forms;
namespace LabelTextColorSample
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int valueAsString = (Int16)value;
            switch (valueAsString)
            {
                case (1):
                    {
                        return Color.Black;
                    }
                case (0):
                    {
                        return Color.Green;
                    }
                default:
                    {
                        return Color.Green;//FromHex(value.ToString());
                    }
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}