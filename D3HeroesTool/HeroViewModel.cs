using D3Data;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace D3HeroesTool
{
    public class HeroViewModel : INotifyPropertyChanged
    {
        private WebClient wc = new WebClient();

        private HeroSummary _currentHero;
        ImageSource _bgSource;

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

        public string Name
        {
            get
            {
                if (CurrentHero == null)
                    return null;
                return CurrentHero.name;
            }
            set
            {
                CurrentHero.name = value;
                OnPropertyChanged("Name");
            }
        }

        public ImageSource Background
        {
            get
            {
                // Handle fast cases (we already have our image, or it is currently being downloaded)
                if (_bgSource != null || wc.IsBusy)
                    return _bgSource;

                // Builds local path to image (requires getting the hero's class in a "bnet-io-friendly" format for WD/DH)
                // thus exploiting reflection to get the serialization string defined in the enum
                var classType = typeof(D3Class);
                var memberInfo = classType.GetMember(CurrentHero.d3class.ToString());
                var attribs = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
                string d3className = attribs.Length > 0 ?
                      ((EnumMemberAttribute)attribs[0]).Value
                    : CurrentHero.d3class.ToString();

                string bgName = String.Format("{0}-{1}.jpg", d3className, CurrentHero.gender.ToString()).ToLower();
                string bgPath = "cache/static/" + bgName;
                Directory.CreateDirectory("cache/static/");

                // Tag our image as currently being requested, and starts downloading it
                FileInfo fi = new FileInfo(bgPath);
                if (!File.Exists(bgPath) || fi.Length == 0)
                {
                    string url = "http://eu.battle.net/d3/static/images/profile/hero/paperdoll/" + bgName;
                    wc.DownloadFileTaskAsync(url, bgPath)
                        .ContinueWith(task =>
                            {
                                if (task.IsCompleted)
                                {
                                    // If download is really successful, tell WPF our property has changed to display it
                                    if (File.Exists(bgPath) && fi.Length > 0)
                                    {
                                        _bgSource = new BitmapImage(new Uri(bgPath, UriKind.Relative));
                                        OnPropertyChanged("Background");
                                    }
                                }
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                    return null;
                }

                _bgSource = new BitmapImage(new Uri(bgPath, UriKind.Relative));
                return _bgSource;
            }
            set
            {
                _bgSource = value;
                OnPropertyChanged("Background");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        private void Reset()
        {
            _bgSource = null;
        }
    }
}
