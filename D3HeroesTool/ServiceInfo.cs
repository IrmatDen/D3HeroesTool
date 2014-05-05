using D3Data;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace D3HeroesTool
{
    [DataContract]
    public class ServiceInfo : INotifyPropertyChanged
    {
        private D3Data.Server _server;
        private D3Data.Locale _locale;
        private string _battletag;

        [DataMember(Name = "Server")]
        public D3Data.Server Server
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged("Server");
            }
        }

        [DataMember(Name = "Locale")]
        public D3Data.Locale Locale
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
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
    }
}
