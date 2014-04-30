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
        private string BattleTagAccessor;

        public FSBNetService()
        { }

        public void Setup(D3Data.Server s, string battleTag, D3Data.Locale l = D3Data.Locale.en_US)
        {
            BattleTagAccessor = battleTag.Replace('#', '-').ToLower();
        }

        public void GetCareer(Action<string> onCareerJSonReceived, Action onError)
        {
            string careerPath = RootFolder + '\\' + BattleTagAccessor;
            if (!File.Exists(careerPath))
                onCareerJSonReceived(File.ReadAllText(careerPath));
            else
                onError();
        }
    }
}
