using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3HeroesTool
{
    class FSBNetService : D3Data.IBNetService
    {
        public string RootFolder { get; set; }

        private string battleTagAccessor;
        private D3Data.Server server;
        private D3Data.Locale locale;

        public FSBNetService()
        { }

        public void Setup(D3Data.Server s, string battleTag, D3Data.Locale l = D3Data.Locale.en_US)
        {
            battleTagAccessor = battleTag.Replace('#', '-').ToLower();
            server = s;
            locale = l;
        }

        public void GetCareer(Action<string> onCareerJSonReceived, Action onError)
        {
            string careerPath = Path.Combine(RootFolder, locale.ToString(), server.ToString(), battleTagAccessor);
            careerPath = Path.ChangeExtension(careerPath, "json");
            if (File.Exists(careerPath))
                onCareerJSonReceived(File.ReadAllText(careerPath));
            else
                onError();
        }
    }
}
