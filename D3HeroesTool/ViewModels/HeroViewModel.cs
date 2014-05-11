using D3Data;
using System;
using System.Windows.Media;

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
            get
            {
                if (CurrentHero == null || String.IsNullOrEmpty(CurrentHero.name))
                    return null;
                return CurrentHero.name.Substring(0, 1).ToUpper();
            }
        }
        public string NameOtherLetters
        {
            get
            {
                if (CurrentHero == null || CurrentHero.name.Length < 2)
                    return null;
                return CurrentHero.name.Substring(1).ToUpper();
            }
        }

        public ImageSource Background
        {
            get
            {
                if (CurrentHero == null)
                    return null;

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
