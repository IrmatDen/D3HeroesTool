using D3Data;
using System.Collections;
using System.Windows.Media;

namespace D3HeroesTool.ViewModels
{
    public class CareerViewModel : BaseViewModel
    {
        private Career _career;
        private HeroSummary _heroSummary;
        private HeroViewModel _heroVM;
        private ImageSource _portraitsImg;
        private ImageSource _tabStates;

        #region Properties
        public Career Career
        {
            get { return _career; }
            set
            {
                _career = value;
                Hero = _career.lastHeroPlayed;
                OnPropertyChanged("Career");
                OnPropertyChanged("Heroes");
            }
        }

        public ImageSource Portraits
        {
            get { return _portraitsImg; }
            set
            {
                _portraitsImg = value;
                OnPropertyChanged("Portraits");
            }
        }

        public ImageSource TabStates
        {
            get { return _tabStates; }
            set
            {
                _tabStates = value;
                OnPropertyChanged("TabStates");
            }
        }

        /// <summary>
        /// Get all heroes from career
        /// </summary>
        public IEnumerable Heroes
        {
            get { return Career.heroes; }
        }

        /// <summary>
        /// Current hero selected (defaults to the last hero played after career is retrieved)
        /// </summary>
        public HeroSummary Hero
        {
            get
            {
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
                return _heroVM ?? (_heroVM = new HeroViewModel());
            }
            set
            {
                _heroVM = value;
                OnPropertyChanged("HeroVM");
            }
        }
        #endregion
    }
}
