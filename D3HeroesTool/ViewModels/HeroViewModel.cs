using D3Data;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace D3HeroesTool.ViewModels
{
    public class HeroViewModel : BaseViewModel
    {
        private HeroSummary _currentHero;
        ImageSource _bgSource;
        bool _bgSourceRequested;

        public HeroSummary CurrentHero
        {
            get { return _currentHero; }
            set
            {
                _currentHero = value;
                Reset();
                OnPropertyChanged(null);
            }
        }

        public string NameFirstLetter
        {
            get { return CurrentHero.name.Substring(0, 1).ToUpper(); }
        }
        public string NameOtherLetters
        {
            get
            {
                if (CurrentHero.name.Length < 2)
                    return null;
                return CurrentHero.name.Substring(1).ToUpper();
            }
        }

        public ImageSource Background
        {
            get
            {
                if (_bgSource == null && !_bgSourceRequested)
                {
                    _bgSourceRequested = true;
                    App.FSProvider.GetBackground(CurrentHero.d3class, CurrentHero.gender,
                        (img) => Background = img,
                        () => { }
                        );
                }

                return _bgSource;
            }
            set
            {
                _bgSource = value;
                _bgSourceRequested = false;
                OnPropertyChanged("Background");
            }
        }

        private void Reset()
        {
            _bgSource = null;
            _bgSourceRequested = false;
        }
    }
}
