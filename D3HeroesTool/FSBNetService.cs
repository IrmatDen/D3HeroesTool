using D3Data;
using D3Data.Utils;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace D3HeroesTool
{
    public class FSBNetService : IBNetService
    {
        private static readonly TimeSpan ObsolescenceDelay = new TimeSpan(12, 0, 0);

        private IBNetService WebAccessor;
        private string battleTagAccessor;
        private Server server;
        private Locale locale;

        public string RootFolder { get; set; }

        public FSBNetService(IBNetService webAccessor)
        {
            WebAccessor = webAccessor;
        }

        public void Setup(Server s, string battleTag, Locale l = Locale.en_US)
        {
            WebAccessor.Setup(s, battleTag, l);

            battleTagAccessor = battleTag.Replace('#', '-').ToLower();
            server = s;
            locale = l;
        }

        public void GetCareer(Action<string> onCareerJSonReceived, Action onError)
        {
            string careerFolder = Path.Combine(RootFolder, locale.ToString(), server.ToString());
            string careerPath = Path.ChangeExtension(Path.Combine(careerFolder, battleTagAccessor), "json");

            if (File.Exists(careerPath) && !IsFileOutdated(careerPath))
                onCareerJSonReceived(File.ReadAllText(careerPath));
            else
            {
                Directory.CreateDirectory(careerFolder);
                WebAccessor.GetCareer(
                    (string json) => {
                        File.WriteAllText(careerPath, json);
                        onCareerJSonReceived(json);
                    },
                    onError);
            }
        }

        public void GetBackground(HeroSummary hero, Action<BitmapImage> onImgReceived, Action onError)
        {
            string d3className = Misc.GetBackgroundNameForClass(hero.d3class);
            string bgName = String.Format("{0}-{1}.jpg", d3className, hero.gender.ToString()).ToLower();
            ServeImage(bgName, onImgReceived, onError,
                (rcv_evt, err) => WebAccessor.GetBackground(hero, rcv_evt, err));
        }

        public void GetPortraits(Action<BitmapImage> onImgReceived, Action onError)
        {
            ServeImage("hero-nav-portraits.jpg", onImgReceived, onError, WebAccessor.GetPortraits);
        }

        public void GetTabStates(Action<BitmapImage> onImgReceived, Action onError)
        {
            ServeImage("hero-nav-frames.png", onImgReceived, onError, WebAccessor.GetTabStates);
        }

        private static bool IsFileOutdated(string filepath)
        {
            TimeSpan timespan = DateTime.UtcNow - File.GetLastWriteTimeUtc(filepath);
            return timespan > ObsolescenceDelay;
        }

        private void ServeImage(string imageFileName, Action<BitmapImage> onImgReceived, Action onError,
                                Action<Action<BitmapImage>, Action> webFallbackCall)
        {
            string bgPath = Path.Combine(RootFolder, "static");
            Directory.CreateDirectory(bgPath);

            bgPath = Path.Combine(bgPath, imageFileName);

            FileInfo fi = new FileInfo(bgPath);
            if (File.Exists(bgPath) && fi.Length > 0)
                onImgReceived(new BitmapImage(new Uri(bgPath, UriKind.Relative)));
            else
            {
                webFallbackCall(
                    (BitmapImage img) =>
                    {
                        using (Stream ostream = new FileStream(bgPath, FileMode.Create))
                        {
                            PngBitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(img));
                            encoder.Save(ostream);
                        }
                        onImgReceived(img);
                    },
                    onError);
            }
        }
    }
}
