using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Data
{
    /// <summary>
    /// Provides D3 json -> D3Data bridges
    /// </summary>
    public class Deserializer
    {
        public static Career AsCareer(string json)
        {
            return JsonConvert.DeserializeObject<Career>(json);
        }
    }
}
