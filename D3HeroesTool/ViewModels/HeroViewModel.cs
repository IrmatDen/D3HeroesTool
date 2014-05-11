using D3Data;
using System;
using System.Windows.Media;

namespace D3HeroesTool.ViewModels
{
    public class HeroViewModel : BaseViewModel
    {
        #region Properties
        private HeroSummary _currentHero;
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

        #region Hero headers related props
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
        public string HeaderHardcoreSeparator
        {
            get
            {
                if (CurrentHero == null || !CurrentHero.hardcore)
                    return null;
                return "-";
            }
        }
        public string HeaderHardcore
        {
            get
            {
                if (CurrentHero == null || !CurrentHero.hardcore)
                    return null;
                return "Hardcore";
            }
        }
        #endregion

        ImageSource _bgSource;
        bool _bgSourceRequested;
        public ImageSource Background
        {
            get
            {
                if (CurrentHero == null)
                    return null;

                if (_bgSource == null && !_bgSourceRequested)
                {
                    _bgSourceRequested = true;
                    App.FSProvider.GetBackground(CurrentHero,
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
        #endregion

        private void Reset()
        {
            _bgSource = null;
            _bgSourceRequested = false;
        }
    }
}
