﻿using D3Data;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace D3HeroesTool.Utils
{
    public class HeroPortraitSubRectConverter : IMultiValueConverter
    {
        private static readonly int PortraitWidth = 83;
        private static readonly int PortraitHeight = 66;

        // Will store <x, y> offsets of portraits
        private static Tuple<int, int>[,] coords;

        private static readonly int[] Xs = new int[] { 0 /*male*/, PortraitWidth /*female*/ };
        private static readonly int[] Ys = new int[] { 0 /*barbarian*/, PortraitWidth /*female*/ };

        private static void BuildPortraitsCoords()
        {
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

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (coords == null)
                BuildPortraitsCoords();

            // values checks
            if (values.Length < 2 || values[0].GetType() != typeof(HeroSummary) || values[1].GetType() != typeof(BitmapImage))
            {
                throw new Exception(String.Format("{0} expects 2 objects of types {1} and {2}",
                    typeof(HeroPortraitSubRectConverter).ToString(),
                    typeof(HeroSummary).ToString(),
                    typeof(BitmapImage).ToString()));
            }

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

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
