using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace D3HeroesTool
{
    class ServiceInfo : INotifyPropertyChanged
    {
        private D3Data.Server _server;
        private D3Data.Locale _locale;
        private string _battletag;

        public D3Data.Server Server
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged("Server");
            }
        }

        public D3Data.Locale Locale
        {
            get { return _locale; }
            set
            {
                _locale = value;
                OnPropertyChanged("Locale");
            }
        }

        public string BattleTag
        {
            get { return _battletag; }
            set
            {
                _battletag = value;
                OnPropertyChanged("BattleTag");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceInfo si = new ServiceInfo();

        public MainWindow()
        {
            InitializeComponent();

            pnlServiceInfo.DataContext = si;
            tbDebug.DataContext = si;
        }
    }
}
