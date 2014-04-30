using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Data
{
    public enum Server
    {
        [Attributes.DisplayString(ResourceKey="D3Server_US")]
        us,
        [Attributes.DisplayString(ResourceKey = "D3Server_EU")]
        eu,
        [Attributes.DisplayString(ResourceKey = "D3Server_TW")]
        tw,
        [Attributes.DisplayString(ResourceKey = "D3Server_KR")]
        kr,
        [Attributes.DisplayString(ResourceKey = "D3Server_CN")]
        cn
    }

    public enum Locale
    {
        en_US,
        en_GB,
        es_MX,
        es_ES,
        it_IT,
        pt_PT,
        fr_FR,
        de_DE,
        pl_PL,
        ru_RU,
        ko_KR,
        zh_TW,
        zh_CN
    };

    public interface IBNetService
    {
        void Setup(Server s, string battleTag, Locale l = Locale.en_US);
        void GetCareer(Action<string> onCareerJSonReceived, Action onError);
    }
}
