using D3Data.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Data;
using WPFLocalizeExtension.Engine;

namespace D3HeroesTool.Utils
{
    /// <summary>
    /// Allows automatic conversion from an enum to it's corresponding DisplayStringAttributes
    /// Based on http://www.ageektrapped.com/blog/the-missing-net-7-displaying-enums-in-wpf/
    /// (using bimap instead of 2 IDictionaries, mainly as an exercise, and a bit for brevity)
    /// </summary>
    public class EnumDisplayer : IValueConverter
    {
        private Type type;
        private IBiMap enumMappings; // Will store <string, enum_values>

        public string ResourcesAssembly { get; set; }
        public string ResourcesRootName { get; set; }
        public Type Type
        {
            get { return type; }
            set
            {
                if (!value.IsEnum)
                    throw new Exception(String.Format("{0} is not an enumeration", value.ToString()));
                type = value;
            }
        }

        public ReadOnlyCollection<string> DisplayNames
        {
            get
            {
                Type enumMappingsType = typeof(SimpleBiMap<,>).GetGenericTypeDefinition().MakeGenericType(typeof(string), type);
                enumMappings = Activator.CreateInstance(enumMappingsType) as IBiMap;

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (var field in fields)
                {
                    DisplayStringAttribute[] a = field.GetCustomAttributes<DisplayStringAttribute>() as DisplayStringAttribute[];
                    string displayValue = GetDisplayStringValue(a);
                    object enumValue = field.GetValue(null);

                    if (displayValue == null)
                        displayValue = enumValue.ToString();

                    if (displayValue != null)
                        enumMappings.Add(displayValue, enumValue);
                }

                return new List<string>(enumMappings.LeftValues as IEnumerable<string>).AsReadOnly();
            }
        }

        private string GetDisplayStringValue(DisplayStringAttribute[] a)
        {
            if (a == null || a.Length == 0)
                return null;

            DisplayStringAttribute dsa = a[0];
            if (!string.IsNullOrEmpty(dsa.ResourceKey))
            {
                return (string)LocalizeDictionary.Instance.GetLocalizedObject(ResourcesAssembly, ResourcesRootName,
                                                                              dsa.ResourceKey, LocalizeDictionary.Instance.Culture);
            }

            return dsa.Value;
        }

        public EnumDisplayer() { }

        public EnumDisplayer(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Get string from enum value
        /// </summary>
        /// <param name="value">enum value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>string, or throws</returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            object displayedString;
            if (!enumMappings.TryGetByRight(value, out displayedString))
                throw new Exception(String.Format("Unknown enum value {0}", value.ToString()));

            return (string)displayedString;
        }

        /// <summary>
        /// Get enum value from string
        /// </summary>
        /// <param name="value">string expected to be one of a DisplayStringAttribute of enum displayed</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>enum value, or throws</returns>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            object enumValue;
            if (!enumMappings.TryGetByLeft(value, out enumValue))
                throw new Exception(String.Format("Unknown string {0} in enum {1}", value.ToString(), type.ToString()));
            return Convert.ChangeType(enumValue, type);
        }
    }
}
