using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

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

            InitializeComponent();

            pnlServiceInfo.DataContext = si;
            tbDebug.DataContext = si;
        }

        private void btnInvoke_Click(object sender, RoutedEventArgs e)
        {
            saveServiceInfoSettings();

            string errMsg = "Couldn't retrieve your profile. Please check your server and battletag.";
            App.ActiveBNet.Setup(si.Server, si.BattleTag, si.Locale);
            App.ActiveBNet.GetCareer((string json) => { tbDebug.Text = json; },
                                     () => { MessageBox.Show(errMsg, "D3HeroesTool", MessageBoxButton.OK, MessageBoxImage.Error); });
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
