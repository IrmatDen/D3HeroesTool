using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace D3Data
{
    public enum Gender { Male, Female } // sorry, can't be a gentleman here, blizzard decided!

    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum D3Class
    {
        Barbarian,
        Crusader,
        [EnumMember(Value="demon-hunter")]
        DemonHunter,
        Monk,
        [EnumMember(Value = "witch-doctor")]
        WitchDoctor,
        Wizard
    }
}