using D3Data;
using System;
using System.Windows.Media;

namespace D3HeroesTool.ViewModels
{
    public class HeroViewModel : BaseViewModel
    {
        #region Properties
        private Hero _currentHero;
        public Hero CurrentHero
        {
            get { return _currentHero; }
            set
            {
                _currentHero = value;
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

        public ImageSource Background
        {
            get
            {
                if (_currentHero == null)
                    return null;
                return App.BNetService.GetBackground(_currentHero);
            }
        }
        #endregion
    }
}
