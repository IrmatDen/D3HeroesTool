using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Data
{
    public class Career
    {
        public string battleTag { get; set; }
        public int paragonLevel { get; set; }
        public int paragonLevelHardcore { get; set; }
        public int lastHeroPlayed { get; set; }

        [JsonConverter(typeof(Converters.UnixTimestampConverter))]
        public DateTime lastUpdated { get; set; }

        public Career() { }
    }
}
