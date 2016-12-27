using System;
using System.Globalization;
using System.Net.Cache;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BreweryApp.Views.Converters
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri srcUri = null;
            // help the designer
            if(value == null)
            {
                return null;
            }
            string uri = (value.ToString()[0] == '/' ? "pack://application:,,," : "") + value;
            bool isValidUri = Uri.TryCreate(uri, UriKind.Absolute, out srcUri);
            if (!(value is string) || !isValidUri)
            {
                throw new ApplicationException($"La valeur {value.ToString()} doit être une chaîne représentant une URI Absolue.");
            }
            BitmapImage convertedImage = new BitmapImage();
            convertedImage.BeginInit();
            convertedImage.CacheOption = BitmapCacheOption.OnLoad;
            convertedImage.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
            convertedImage.UriSource = srcUri;
            convertedImage.EndInit();
            
            return convertedImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
