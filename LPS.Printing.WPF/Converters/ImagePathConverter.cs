using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace LPS.Printing.WPF
{
    public class ImagePathConverter : IValueConverter
    {
        private static readonly ImagePathConverter _instance = new ImagePathConverter();
        public static ImagePathConverter Instance { get { return _instance; } }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, parameter);
        }

        private object Convert(object value, object parameter)
        {
            if (value is string)
            {
                string relativePath = (parameter ?? string.Empty).ToString();
                var img = new BitmapImage();
                string path = value.ToString();
                if (!string.IsNullOrEmpty(relativePath))
                {
                    if (relativePath == "./")
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                    }
                    else
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            relativePath, path);
                    }
                }
                try
                {
                    if (File.Exists(path) || Uri.CheckSchemeName(path))
                    {
                        return new BitmapImage(new Uri(path, UriKind.Absolute));
                    }
                }
                catch
                {

                }
            }
            return null;
        }
    }
}