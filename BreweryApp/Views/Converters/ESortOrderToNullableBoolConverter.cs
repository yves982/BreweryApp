using BreweryApp.ViewModels.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BreweryApp.Views.Converters
{
    [ValueConversion(typeof(ESortOrder), typeof(bool?))]
    public class ESortOrderToNullableBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? convertedValue = null;
            if(!(value is ESortOrder))
            {
                throw new ArgumentException("L'argument value doit être un SortOrder (SortOrderToNullableBoolConverter).");
            }
            if((ESortOrder)value != ESortOrder.None)
            {
                convertedValue = ((ESortOrder)value) == ESortOrder.Desc;
            }
            return convertedValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ESortOrder convertedOrder = ESortOrder.None;
            if(!(value is bool) && value != null)
            {
                throw new ArgumentException("L'argument value doit être un bool? (SortOrderToNullableBoolConverter).");
            }
            bool? currentValue = (bool?)value;
            if (currentValue.HasValue)
            {
                convertedOrder = currentValue.Value ? ESortOrder.Desc : ESortOrder.Asc;
            }
            
            return convertedOrder;
        }
    }
}
