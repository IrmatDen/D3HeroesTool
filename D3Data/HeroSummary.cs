using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace D3Data
{
    public class HeroSummary
    {
        public int id { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public bool hardcore{ get; set; }
        public bool dead { get; set; }
        public Gender gender { get; set; }

        public HeroSummary() { }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            if (gender != Gender.Female && gender != Gender.Male)
            {
                throw new IndexOutOfRangeException(String.Format("Unexpected gender parsed. Expected 0 or 1, got {0}.", gender));
            }
        }
    }
}
