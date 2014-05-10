using D3Data;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using WPFLocalizeExtension.Engine;

namespace D3HeroesTool
{
    [DataContract]
    public class ServiceInfo : INotifyPropertyChanged
    {
        private static readonly string SettingsFileName = "./settings.d3ht";

        private Server _server;
        private Locale _locale;
        private string _battletag;
        private Career _career;
        private HeroSummary _heroSummary;
        private HeroViewModel _heroVM;

        #region Properties
        [DataMember(Name = "Server")]
        public Server Server
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged("Server");
            }
        }

        [DataMember(Name = "Locale")]
        public Locale Locale
        {
            get { return _locale; }
            set
            {
                _locale = value;
                OnPropertyChanged("Locale");
            }
        }

        [DataMember(Name = "BattleTag")]
        public string BattleTag
        {
            get { return _battletag; }
            set
            {
                _battletag = value;
                OnPropertyChanged("BattleTag");
            }
        }

        public Career Career
        {
            get { return _career; }
            set
            {
                _career = value;
                Hero = _career.lastHeroPlayed;
                OnPropertyChanged("Career");
            }
        }

        /// <summary>
        /// Current hero selected (defaults to the last hero played after career is retrieved)
        /// </summary>
        public HeroSummary Hero
        {
            get{
                if (_heroSummary == null && _career != null)
                    _heroSummary = _career.lastHeroPlayed;
                return _heroSummary;
            }
            set
            {
                _heroSummary = value;
                HeroVM.CurrentHero = _heroSummary;
                OnPropertyChanged("Hero");
            }
        }

        public HeroViewModel HeroVM
        {
            get
            {
                if (_heroVM == null)
                    _heroVM = new HeroViewModel();
                return _heroVM;
            }
            set
            {
                _heroVM = value;
                OnPropertyChanged("HeroVM");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        #endregion

        #region Interactions
        public bool CanLoadCareer()
        {
            return true;
        }

        public void LoadCareer()
        {
            saveSettings();

            string errMsg = (string)LocalizeDictionary.Instance.GetLocalizedObject("D3HeroesTool", "ResourceStrings", "errRetrievingProfile",
                                                                                   LocalizeDictionary.Instance.Culture);
            App.FSProvider.Setup(Server, BattleTag, Locale);
            App.FSProvider.GetCareer(
                (string json) => { Career = D3Data.Deserializer.AsCareer(json); },
                () => { MessageBox.Show(errMsg, "D3HeroesTool", MessageBoxButton.OK, MessageBoxImage.Error); }
                );
        }
        #endregion

        #region Save/restore
        /// <summary>
        /// Initialize a new ServiceInfo instance based on saved settings (if any).
        /// </summary>
        static public ServiceInfo tryRestoreSettings()
        {
            if (!File.Exists(SettingsFileName))
                return new ServiceInfo();

            ServiceInfo instance;
            try
            {
                DataContractSerializer deserializer = new DataContractSerializer(typeof(ServiceInfo));
                using (Stream inStream = new FileStream(SettingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    instance = (ServiceInfo)deserializer.ReadObject(inStream);
                    string bnetLocale = instance.Locale.ToString().Replace("_", "-");
                    LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(bnetLocale);
                }
            }
            catch (Exception)
            {
                instance = new ServiceInfo();
            }

            return instance;
        }

        /// <summary>
        /// Dump current settings
        /// </summary>
        public void saveSettings()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ServiceInfo));
            using (Stream outStream = new FileStream(SettingsFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.WriteObject(outStream, this);
            }
        }
#endregion
    }
}
