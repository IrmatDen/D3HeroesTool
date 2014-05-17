using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        [Attributes.DisplayString(ResourceKey = "D3Loc_en_US")]
        en_US,
        [Attributes.DisplayString(ResourceKey = "D3Loc_en_GB")]
        en_GB,
        [Attributes.DisplayString(ResourceKey = "D3Loc_es_MX")]
        es_MX,
        [Attributes.DisplayString(ResourceKey = "D3Loc_es_ES")]
        es_ES,
        [Attributes.DisplayString(ResourceKey = "D3Loc_it_IT")]
        it_IT,
        [Attributes.DisplayString(ResourceKey = "D3Loc_pt_PT")]
        pt_PT,
        [Attributes.DisplayString(ResourceKey = "D3Loc_fr_FR")]
        fr_FR,
        [Attributes.DisplayString(ResourceKey = "D3Loc_de_DE")]
        de_DE,
        [Attributes.DisplayString(ResourceKey = "D3Loc_pl_PL")]
        pl_PL,
        [Attributes.DisplayString(ResourceKey = "D3Loc_ru_RU")]
        ru_RU,
        [Attributes.DisplayString(ResourceKey = "D3Loc_ko_KR")]
        ko_KR,
        [Attributes.DisplayString(ResourceKey = "D3Loc_zh_TW")]
        zh_TW,
        [Attributes.DisplayString(ResourceKey = "D3Loc_zh_CN")]
        zh_CN
    };

    public interface IBNetService
    {
        void Setup(Server s, string battleTag, Locale l = Locale.en_US);

        // Data downloads
        void GetCareer(Action<string> onCareerJSonReceived, Action onError);

        // Media downloads
        ImageSource GetBackground(HeroSummary hero);
        ImageSource GetPortraits();
        ImageSource GetTabStates();
    }
}
