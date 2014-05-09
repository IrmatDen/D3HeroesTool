using D3HeroesTool.Utils;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFLocalizeExtension.Engine;

namespace D3HeroesTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string SettingsFileName = "./settings.d3ht";

        ServiceInfo si = new ServiceInfo();

        public MainWindow()
        {
            tryLoadServiceInfoSettings();

            si.PropertyChanged += (sender, args) => { if (args.PropertyName == "Locale") updateCulture(); };

            InitializeComponent();

            DataContext = si;
        }

        private void updateCulture()
        {
            string bnetLocale = si.Locale.ToString().Replace("_", "-");
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(bnetLocale);

            // Binded through an IValueConverter, so we have to trigger update ourselves
            cbServer.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateTarget();
            cbServer.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateTarget();

            // NB: we don't update cbLocale since strings are already localized (+ it would trigger infinite recursion)
        }

        private void btnInvoke_Click(object sender, RoutedEventArgs e)
        {
            saveServiceInfoSettings();

            string errMsg = (string)LocalizeDictionary.Instance.GetLocalizedObject("D3HeroesTool", "ResourceStrings", "errRetrievingProfile",
                                                                                   LocalizeDictionary.Instance.Culture);
            App.FSProvider.Setup(si.Server, si.BattleTag, si.Locale);
            App.FSProvider.GetCareer(
                (string json) => {
                    si.Career = D3Data.Deserializer.AsCareer(json);
                },
                () => {
                    MessageBox.Show(errMsg, "D3HeroesTool", MessageBoxButton.OK, MessageBoxImage.Error);
                });
        }

        private void tryLoadServiceInfoSettings()
        {
            if (!File.Exists(SettingsFileName))
                return;

            try
            {
                DataContractSerializer deserializer = new DataContractSerializer(typeof(ServiceInfo));
                using (Stream inStream = new FileStream(SettingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    si = (ServiceInfo)deserializer.ReadObject(inStream);
                    string bnetLocale = si.Locale.ToString().Replace("_", "-");
                    LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(bnetLocale);
                }
            }
            catch(Exception)
            {
                si = new ServiceInfo();
            }
        }

        private void saveServiceInfoSettings()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ServiceInfo));
            using (Stream outStream = new FileStream(SettingsFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.WriteObject(outStream, si);
            }
        }
    }
}
