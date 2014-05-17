using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace D3HeroesTool.Converters
{
    /// <summary>
    /// Forwards a ValidationError's message as a string value
    /// </summary>
    public class ValidationErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var err = value as ValidationError;
            if (err != null)
                return err.ErrorContent;
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
