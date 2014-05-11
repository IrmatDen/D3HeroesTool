using D3Data;
using D3Data.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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

        public void GetBackground(D3Class desiredClass, Gender desiredGender, Action<BitmapImage> onImgReceived, Action onError)
        {
            string d3className = Misc.GetBackgroundNameForClass(desiredClass);

            string bgName = String.Format("{0}-{1}.jpg", d3className, desiredGender.ToString()).ToLower();
            string bgPath = Path.Combine(RootFolder, "static");
            Directory.CreateDirectory(bgPath);

            bgPath = Path.Combine(bgPath, bgName);

            // Tag our image as currently being requested, and starts downloading it
            FileInfo fi = new FileInfo(bgPath);
            if (File.Exists(bgPath) && fi.Length > 0)
                onImgReceived(new BitmapImage(new Uri(bgPath, UriKind.Relative)));
            else
            {
                WebAccessor.GetBackground(desiredClass, desiredGender,
                    (BitmapImage img) =>
                    {
                        using (Stream ostream = new FileStream(bgPath, FileMode.Create))
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(img));
                            encoder.Save(ostream);
                        }
                        onImgReceived(img);
                    },
                    onError);
            }
        }

        private static bool IsFileOutdated(string filepath)
        {
            TimeSpan timespan = DateTime.UtcNow - File.GetCreationTimeUtc(filepath);
            return timespan > ObsolescenceDelay;
        }
    }
}
