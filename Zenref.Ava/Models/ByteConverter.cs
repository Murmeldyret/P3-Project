using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Zenref.Ava.Models
{
    public class ByteConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string size = "0 KB";

            if (value != null)
            {
                double byteCount = 0;

                byteCount = System.Convert.ToDouble(value);

                if (byteCount >= 1073741824)
                {
                    size = String.Format("{0:##}", byteCount / 1073741824) + " GB";
                }
                else if (byteCount >= 1048576)
                {
                    size = String.Format("{0:##}", byteCount / 1048576) + " MB";
                }
                else if (byteCount >= 1024)
                {
                    size = String.Format("{0:##}", byteCount / 1024) + " KB";
                }
                else if (byteCount > 0 && byteCount < 1024)
                    size = "1 KB";
            }
            return size;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
