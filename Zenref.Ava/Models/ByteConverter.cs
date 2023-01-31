using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Zenref.Ava.Models
{
    // ByteConverter is an implementation of the IValueConverter interface 
    // which is used to convert a byte value to a more human-readable format (e.g. KB, MB, GB).
    public class ByteConverter : IValueConverter
    {
        // Convert method takes in an object value, a target type, a parameter and a culture info.
        // The method converts the value to a string representation of the size in KB, MB or GB
        // and returns it.
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string size = "0 KB";
            
            // If the value is not null, it is casted to a double and checked against 
            // different size thresholds (in bytes) to determine the appropriate size unit
            // (KB, MB or GB) and the size is returned as a string.

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
        
        // ConvertBack method is not implemented in this case as it is not needed.
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
