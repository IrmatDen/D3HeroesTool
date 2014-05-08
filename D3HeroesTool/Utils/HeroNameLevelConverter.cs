using D3Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace D3HeroesTool.Utils
{
    /// <summary>
    /// Allows to display an HeroSummary instance as "HeroName (HeroLevel)"
    /// </summary>
    public class HeroNameLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hero = value as HeroSummary;
            if (hero != null)
                return String.Format("{0} ({1})", hero.name, hero.level.ToString());
            return "#CONVERT_FAILED#";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
