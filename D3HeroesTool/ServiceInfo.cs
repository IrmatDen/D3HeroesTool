using D3Data;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace D3HeroesTool
{
    [DataContract]
    public class ServiceInfo : INotifyPropertyChanged
    {
        private Server _server;
        private Locale _locale;
        private string _battletag;
        private Career _career;

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
                OnPropertyChanged("Career");
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
