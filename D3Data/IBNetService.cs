using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Data
{
    public enum Server { us, eu, tw, kr, cn };
    public enum Locale { en_US, en_GB, es_MX, es_ES, it_IT, pt_PT, fr_FR, de_DE, pl_PL, ru_RU, ko_KR, zh_TW, zh_CN };

    public interface IBNetService
    {
        void Setup(Server s, string battleTag, Locale l = Locale.en_US);
        Task GetCareer(Action<string> onCareerJSonReceived, Action onError);
    }
}
