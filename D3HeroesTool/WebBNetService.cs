using D3Data;
using System;
using System.Net;
using System.Threading.Tasks;

namespace D3HeroesTool
{
    public class WebBNetService : IBNetService
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
    }
}
