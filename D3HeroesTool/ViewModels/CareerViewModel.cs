using D3Data;
using System.Collections;
using System.Linq;

namespace D3HeroesTool.ViewModels
{
    public class CareerViewModel : BaseViewModel
    {
        private Career _career;
        private HeroSummary _heroSummary;
        private HeroViewModel _heroVM;

        #region Properties
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
        #endregion
    }
}
