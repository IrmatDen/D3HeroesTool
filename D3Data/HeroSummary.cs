﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace D3Data
{
    public class Hero : IComparable
    {
        public int id { get; set; }
        public string name { get; set; }
        public int paragonLevel { get; set; }
        public int level { get; set; }
        public bool hardcore { get; set; }
        public bool dead { get; set; }
        public Gender gender { get; set; }

        [JsonProperty(PropertyName="last-updated")]
        [JsonConverter(typeof(Converters.UnixTimestampConverter))]
        public DateTime lastUpdated { get; set; }

        [JsonProperty(PropertyName = "class")]
        public D3Class d3class { get; set; }

        public SkillSet skills { get; set; }

        public Hero() { }

        public int CompareTo(Object other)
        {
            return id.CompareTo((other as Hero).id);
        }

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
