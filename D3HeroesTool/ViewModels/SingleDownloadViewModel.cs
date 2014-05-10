
namespace D3HeroesTool.ViewModels
{
    public class SingleDownloadViewModel : BaseViewModel
    {
        string _text;

        public string DownloadCaption
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged("DownloadCaption");
            }
        }
    }
}
