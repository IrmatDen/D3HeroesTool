using D3Data;
using D3Data.Utils;
using System;
using System.Net;
using System.Windows.Media.Imaging;

namespace D3HeroesTool
{
    public class WebBNetService : IBNetService, IDisposable
    {
        private WebClient wc = new WebClient();

        private string battleTagAccessor;
        private Server server;
        private Locale locale;

        public event DownloadCareerStartedEventHandler OnDownloadCareerStarted;
        public event DownloadCareerFinishedEventHandler OnDownloadCareerFinished;

        public void Setup(Server s, string battleTag, Locale l = Locale.en_US)
        {
            battleTagAccessor = battleTag.Replace('#', '-').ToLower();
            server = s;
            locale = l;
        }

        public void Dispose()
        {
            wc.Dispose();
            GC.SuppressFinalize(this);
        }

        public void GetCareer(Action<string> onCareerJSonReceived, Action onError)
        {
            string careerUrl = "http://{0}.battle.net/api/d3/profile/{1}/?locale={2}";
            careerUrl = String.Format(careerUrl, server.ToString(), battleTagAccessor,
                                      locale.ToString().Replace('_', '-'));

            RaiseStartEvent(new BNetDownloadStartedEventArgs(String.Format("Downloading {0}", battleTagAccessor)));
            BNetDownloadFinishedEventArgs finishedArgs = new BNetDownloadFinishedEventArgs();

            wc.DownloadStringTaskAsync(careerUrl)
                .ContinueWith(task =>
                {
                    RaiseFinishedEvent(finishedArgs);

                    if (!task.IsFaulted)
                    {
                        // Basic validation on what we got
                        if (task.Result.Contains("\"reason\" : \"The account could not be found.\""))
                            onError();
                        else
                            onCareerJSonReceived(task.Result);
                    }
                    else
                        onError();
                });
        }

        public void GetBackground(HeroSummary hero, Action<BitmapImage> onImgReceived, Action onError)
        {
            string d3className = Misc.GetBackgroundNameForClass(hero.d3class);
            string bgName = String.Format("{0}-{1}.jpg", d3className, hero.gender.ToString()).ToLower();
            string url = "http://eu.battle.net/d3/static/images/profile/hero/paperdoll/" + bgName;
            DownloadImage(url, onImgReceived, onError);
        }

        public void GetPortraits(Action<BitmapImage> onImgReceived, Action onError)
        {
            string url = "http://eu.battle.net/d3/static/images/profile/hero/hero-nav-portraits.jpg";
            DownloadImage(url, onImgReceived, onError);
        }

        public void GetTabStates(Action<BitmapImage> onImgReceived, Action onError)
        {
            string url = "http://eu.battle.net/d3/static/images/profile/hero/hero-nav-frames.png";
            DownloadImage(url, onImgReceived, onError);
        }

        private void DownloadImage(string url, Action<BitmapImage> onImgReceived, Action onError)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            // Looks like caching gets in the way and deny access to image (rare phenomenon, but still 5-10% of the time)
            // Since caching is done through FSBNetService, might as well disable that until more is understood...
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.DownloadCompleted += (o, args) => onImgReceived(image);
            image.DownloadFailed += (o, args) => onError();
            image.UriSource = new Uri(url);
            image.EndInit();
        }

        #region Events
        private void RaiseFinishedEvent(BNetDownloadFinishedEventArgs args)
        {
            if (OnDownloadCareerFinished != null)
                OnDownloadCareerFinished(this, args);
        }

        private void RaiseStartEvent(BNetDownloadStartedEventArgs args)
        {
            if (OnDownloadCareerStarted != null)
                OnDownloadCareerStarted(this, args);
        }
        #endregion
    }
}
