using D3Data;
using D3Data.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace D3HeroesTool
{
    public class WebBNetService : IBNetService, IDisposable
    {
        private WebClient wc = new WebClient();

        private string battleTagAccessor;
        private Server server;
        private Locale locale;

        private Dictionary<string, BitmapImage> backgrounds = null;

        public event DownloadCareerStartedEventHandler OnDownloadCareerStarted;
        public event DownloadCareerFinishedEventHandler OnDownloadCareerFinished;

        public void Setup(Server s, string battleTag, Locale l = Locale.en_US)
        {
            battleTagAccessor = battleTag.Replace('#', '-').ToLower();
            server = s;
            locale = l;

            CacheBackgrounds();
        }

        private void CacheBackgrounds()
        {
            if (backgrounds != null)
                return;

            backgrounds = new Dictionary<string, BitmapImage>();
            foreach (var c in typeof(D3Class).GetEnumValues())
            {
                string d3className = Misc.GetBackgroundNameForClass((D3Class)c);

                string female = Gender.Female.ToString();
                string key = (d3className + "-" + female).ToLower();
                string url = "http://eu.battle.net/d3/static/images/profile/hero/paperdoll/" + key + ".jpg";
                Application.Current.Dispatcher.Invoke(() => backgrounds.Add(key, new BitmapImage(new Uri(url))));

                string male = Gender.Male.ToString();
                key = (d3className + "-" + male).ToLower();
                url = "http://eu.battle.net/d3/static/images/profile/hero/paperdoll/" + key + ".jpg";
                Application.Current.Dispatcher.Invoke(() => backgrounds.Add(key, new BitmapImage(new Uri(url))));
            }
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

        public ImageSource GetBackground(HeroSummary hero)
        {
            string d3className = Misc.GetBackgroundNameForClass(hero.d3class);
            string bgKey = String.Format("{0}-{1}", d3className, hero.gender.ToString()).ToLower();

            BitmapImage img = null;
            if (!backgrounds.TryGetValue(bgKey, out img))
                return null;

            return img;
        }

        public ImageSource GetPortraits()
        {
            return DownloadImage("http://eu.battle.net/d3/static/images/profile/hero/hero-nav-portraits.jpg");
        }

        public ImageSource GetTabStates()
        {
            return DownloadImage("http://eu.battle.net/d3/static/images/profile/hero/hero-nav-frames.png");
        }

        private ImageSource DownloadImage(string url)
        {
            return new BitmapImage(new Uri(url));
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
