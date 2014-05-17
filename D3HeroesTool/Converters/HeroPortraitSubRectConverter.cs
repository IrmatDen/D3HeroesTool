using D3Data;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace D3HeroesTool.Converters
{
    /// <summary>
    /// Converter used to get a region of a BitmapImage based on Hero
    /// </summary>
    public class HeroPortraitSubRectConverter : IMultiValueConverter
    {
        private static readonly int PortraitWidth = 83;
        private static readonly int PortraitHeight = 66;

        /// <summary>
        /// Will store <x, y> tuple offsets of portraits
        /// </summary>
        private static Tuple<int, int>[,] coords;

        /// <summary>
        /// Builds the array containing all expected coordinates based on D3 classes and genders types
        /// </summary>
        private static void BuildPortraitsCoords()
        {
            if (coords != null)
                return;

            coords = new Tuple<int, int>[Enum.GetNames(typeof(D3Class)).Length, Enum.GetNames(typeof(Gender)).Length];

            Action<D3Class, int> genCoord = (c, y) =>
                {
                    coords[(int)c, (int)Gender.Male] = new Tuple<int, int>(0, y);
                    coords[(int)c, (int)Gender.Female] = new Tuple<int, int>(PortraitWidth, y);
                };

            D3Class[] portraits_order = new D3Class[] { D3Class.Barbarian, D3Class.DemonHunter, D3Class.Monk,
                                                        D3Class.WitchDoctor, D3Class.Wizard, D3Class.Crusader };

            for (int idx = 0; idx < portraits_order.Length; idx++)
                genCoord(portraits_order[idx], PortraitHeight * idx);
        }

        /// <summary>
        /// Generates a CroppedBitmap for a given BitmapImage and Hero which should contain the matching portrait
        /// </summary>
        /// <param name="values">Objects array expected to contains a Hero instance and the BitmapImage containing portraits</param>
        /// <param name="targetType">unused</param>
        /// <param name="parameter">unused</param>
        /// <param name="culture">unused</param>
        /// <returns>A CroppedBitmap containing the Hero's portrait or null</returns>
        /// <exception cref="ArgumentException">Thrown if any or all of <paramref name="values"/> is invalid</exception>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values checks
            if (values.Length != 2)
            {
                throw new ArgumentException(String.Format("{0} expects 2 objects of types {1} and {2}",
                    typeof(HeroPortraitSubRectConverter).ToString(),
                    typeof(HeroSummary).ToString(),
                    typeof(BitmapImage).ToString()));
            }

            // null values? woopsie, something went wrong, but try not to crash (at least not yet)
            if (values[0] == null || values[1] == null)
                return null;

            // values checks
            if (values[0].GetType() != typeof(HeroSummary) || values[1].GetType() != typeof(BitmapImage))
            {
                throw new ArgumentException(String.Format("{0} expects 2 objects of types {1} and {2}",
                    typeof(HeroPortraitSubRectConverter).ToString(),
                    typeof(HeroSummary).ToString(),
                    typeof(BitmapImage).ToString()));
            }

            BuildPortraitsCoords();

            var hero = values[0] as HeroSummary;
            var img = values[1] as BitmapImage;
            if (hero != null && img != null)
            {
                var xy = coords[(int)hero.d3class, (int)hero.gender];
                Int32Rect portraitRegion = new Int32Rect(xy.Item1, xy.Item2, PortraitWidth, PortraitHeight);
                return new CroppedBitmap(img, portraitRegion);
            }
            return null;
        }

        /// <summary>
        /// Not implemented: no need to get back hero nor BitmapImage from a CroppedBitmap
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
