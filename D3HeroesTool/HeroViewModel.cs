using D3Data;
using System.ComponentModel;

namespace D3HeroesTool
{
    public class HeroViewModel : INotifyPropertyChanged
    {    
        private HeroSummary _currentHero;
        public HeroSummary CurrentHero
        {
            get { return _currentHero; }
            set
            {
                _currentHero = value;
                OnPropertyChanged(null);
            }
        }

        public string Name
        {
            get
            {
                if (_currentHero == null)
                    return null;
                return _currentHero.name;
            }
            set
            {
                _currentHero.name = value;
                OnPropertyChanged("Name");
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
