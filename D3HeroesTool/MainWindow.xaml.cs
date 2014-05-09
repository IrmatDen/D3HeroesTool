using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using WPFLocalizeExtension.Engine;

namespace D3HeroesTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceInfo si;

        public MainWindow()
        {
            si = ServiceInfo.tryRestoreSettings();

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
            si.saveSettings();

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
    }
}
