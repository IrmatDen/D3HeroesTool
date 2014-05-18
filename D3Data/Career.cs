using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace D3Data
{
    public class Career
    {
        public string battleTag { get; set; }
        public int paragonLevel { get; set; }
        public int paragonLevelHardcore { get; set; }

        [JsonProperty(Order=2)]
        [JsonConverter(typeof(Converters.HeroRefConverter))]
        public Hero lastHeroPlayed { get; set; }

        [JsonConverter(typeof(Converters.UnixTimestampConverter))]
        public DateTime lastUpdated { get; set; }

        public KillStats kills { get; set; }

        [JsonProperty(Order = 1)]
        public SortedSet<Hero> heroes { get; set; }

        public Career() { }
    }
}
