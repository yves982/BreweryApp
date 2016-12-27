using BreweryApp.ViewModels.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BreweryApp.Views.Converters
{
    [ValueConversion(typeof(int), typeof(Page))]
    public class IntToPageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (! (value is int))
            {
                throw new ArgumentException("Le paramètre value doit être un entier (IntToPageConverter).");
            }
            Page page = new Page { Id = (int)value };
            return page;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
