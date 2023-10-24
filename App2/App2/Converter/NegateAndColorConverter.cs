using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace App2.Converter
{
    public class NegateAndColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                booleanValue = !booleanValue; // Negacja wartości

                // Konwersja na kolor
                return booleanValue ? Color.Black : Color.DarkCyan;
            }

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Implementacja według potrzeb
            return null;
        }
    }
}
