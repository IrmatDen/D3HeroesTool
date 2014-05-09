using D3Data;
using System;
using System.Net;
using System.Threading.Tasks;

namespace D3HeroesTool
{
    class WebBNetService : IBNetService
    {
        private WebClient wc = new WebClient();

        private string battleTagAccessor;
        private Server server;
        private Locale locale;

        public event DownloadStartedEventHandler OnDownloadStarted;
        public event DownloadFinishedEventHandler OnDownloadFinished;

        public void Setup(Server s, string battleTag, Locale l = Locale.en_US)
        {
            battleTagAccessor = battleTag.Replace('#', '-').ToLower();
            server = s;
            locale = l;
        }

        public void GetCareer(Action<string> onCareerJSonReceived, Action onError)
        {
            string careerUrl = "http://{0}.battle.net/api/d3/profile/{1}/?locale={2}";
            careerUrl = String.Format(careerUrl, server.ToString(), battleTagAccessor,
                                      locale.ToString().Replace('_', '-'));

            RaiseStartEvent(new BNetDownloadStartedEventArgs("Career"));
            BNetDownloadFinishedEventArgs finishedArgs = new BNetDownloadFinishedEventArgs();

            wc.DownloadStringTaskAsync(careerUrl)
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        RaiseFinishedEvent(finishedArgs);

                        if (!task.IsFaulted)
                            onCareerJSonReceived(task.Result);
                        else
                            onError();
                    }
                    else
                        onError();
                });
        }

        private void RaiseFinishedEvent(BNetDownloadFinishedEventArgs args)
        {
            if (OnDownloadFinished != null)
                OnDownloadFinished(this, args);
        }

        private void RaiseStartEvent(BNetDownloadStartedEventArgs args)
        {
            if (OnDownloadStarted != null)
                OnDownloadStarted(this, args);
        }
    }
}
