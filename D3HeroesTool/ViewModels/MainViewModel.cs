using D3Data;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace D3HeroesTool.ViewModels
{
    [DataContract]
    public class MainViewModel : BaseViewModel
    {
        private static readonly string SettingsFileName = "./settings.d3ht";

        private Server _server;
        private Locale _locale;
        private string _battletag;
        private CareerViewModel _careerVM;
        private BaseViewModel _currentVM;

        private bool _downloading;

        /// <summary>
        /// DataContractSerializer won't construct the object, so a bit of manual handling is required
        /// </summary>
        void Init()
        {
            _careerVM = new CareerViewModel();

            App.WebProvider.OnDownloadCareerStarted += OnCareerDownloadStarted;
            App.WebProvider.OnDownloadCareerFinished += OnCareerDownloadFinished;
        }

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

        public BaseViewModel ViewModel
        {
            get
            {
                return _currentVM;
            }
            set
            {
                _currentVM = value;
                OnPropertyChanged("ViewModel");
            }
        }

        public bool DownloadInProgress
        {
            get
            {
                return _downloading && _careerVM.Portraits == null;
            }
            set
            {
                _downloading = value;
                OnPropertyChanged("DownloadInProgress");
            }
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
                (string json) => { _careerVM.Career = D3Data.Deserializer.AsCareer(json); ViewModel = _careerVM; },
                () => { MessageBox.Show(errMsg, "D3HeroesTool", MessageBoxButton.OK, MessageBoxImage.Error); }
                );
            
            // Load career's portraits as part of our download process
            App.FSProvider.GetPortraits((img) => { _careerVM.Portraits = img; }, () => { });
        }
        #endregion

        #region Event handling
        private void OnCareerDownloadStarted(object sender, BNetDownloadStartedEventArgs args)
        {
            SingleDownloadViewModel downloadVM = new SingleDownloadViewModel();
            ViewModel = downloadVM;

            DownloadInProgress = true;
            downloadVM.DownloadCaption = args.DownloadType;
        }

        private void OnCareerDownloadFinished(object sender, BNetDownloadFinishedEventArgs args)
        {
            DownloadInProgress = false;
        }
        #endregion

        #region Save/restore
        /// <summary>
        /// Initialize a new ServiceInfo instance based on saved settings (if any).
        /// </summary>
        static public MainViewModel tryRestoreSettings()
        {
            if (!File.Exists(SettingsFileName))
                return new MainViewModel();

            MainViewModel instance;
            try
            {
                DataContractSerializer deserializer = new DataContractSerializer(typeof(MainViewModel));
                using (Stream inStream = new FileStream(SettingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    instance = (MainViewModel)deserializer.ReadObject(inStream);
                    string bnetLocale = instance.Locale.ToString().Replace("_", "-");
                    LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(bnetLocale);
                }
            }
            catch (Exception)
            {
                instance = new MainViewModel();
            }

            instance.Init();
            return instance;
        }

        /// <summary>
        /// Dump current settings
        /// </summary>
        public void saveSettings()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(MainViewModel));
            using (Stream outStream = new FileStream(SettingsFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.WriteObject(outStream, this);
            }
        }
#endregion
    }
}
