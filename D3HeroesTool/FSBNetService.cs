using D3Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3HeroesTool
{
    public class FSBNetService : IBNetService
    {
        private IBNetService WebAccessor;

        public string RootFolder { get; set; }

        private string battleTagAccessor;
        private Server server;
        private Locale locale;

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
            if (File.Exists(careerPath))
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
    }
}
