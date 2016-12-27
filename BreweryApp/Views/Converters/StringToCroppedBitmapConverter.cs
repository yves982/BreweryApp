using System;
using System.Globalization;
using System.Net.Cache;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BreweryApp.Views.Converters
{
    [ValueConversion(typeof(string), typeof(CroppedBitmap), ParameterType = typeof(string))]
    public class StringToCroppedBitmapConverter : IValueConverter
    {
        private static void _validateParameter(object parameter, out int x, out int y, out int width, out int height)
        {
            string[] data = parameter.ToString().Split(' ');

            if(data == null || data.Length != 4)
            {
                string errMessage = "Erreur dans le paramètre de conversion (StringToCroppedBitmapConverter). Attendu : 4 nombres séparés par le caractère espace.";
                throw new ApplicationException(errMessage);
            }

            bool xSuccess = int.TryParse(data[0], out x);
            bool ySuccess = int.TryParse(data[1], out y);
            bool widthSuccess = int.TryParse(data[2], out width);
            bool heightSuccess = int.TryParse(data[3], out height);

            if (!(xSuccess && ySuccess && widthSuccess && heightSuccess))
            {
                string errMessage = "Erreur dans le paramètre de conversion (StringToCroppedBitmapConverter). Attendu : 4 nombres séparés par le caractère espace.";
                throw new ApplicationException(errMessage);
            }
        }

        private static BitmapImage _buildSourceImage(Uri srcUri)
        {
            BitmapImage sourceImage = new BitmapImage();
            sourceImage.BeginInit();
            sourceImage.CacheOption = BitmapCacheOption.OnLoad;
            sourceImage.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
            sourceImage.UriSource = srcUri;
            sourceImage.EndInit();
            return sourceImage;
        }

        private static CroppedBitmap _buildCroppedImage(BitmapImage sourceImage, int x, int y, int width, int height)
        {
            CroppedBitmap croppedBitmap = new CroppedBitmap();
            croppedBitmap.BeginInit();
            croppedBitmap.Source = sourceImage;
            croppedBitmap.SourceRect = new Int32Rect(x, y, width, height);
            croppedBitmap.EndInit();
            return croppedBitmap;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri srcUri = null;
            // help the designer
            if (value == null)
            {
                return null;
            }
            string uri = (value.ToString()[0] == '/' ? "pack://application:,,," : "") + value;
            bool isValidUri = Uri.TryCreate(uri, UriKind.Absolute, out srcUri);
            if (!(value is string) || !isValidUri)
            {
                throw new ApplicationException($"La valeur {value.ToString()} doit être une chaîne représentant une URI Absolue.");
            }

            int x, y, width, height;
            _validateParameter(parameter, out x, out y, out width, out height);

            BitmapImage sourceImage = _buildSourceImage(srcUri);
            CroppedBitmap croppedBitmap = _buildCroppedImage(sourceImage, x, y, width, height);

            return croppedBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
