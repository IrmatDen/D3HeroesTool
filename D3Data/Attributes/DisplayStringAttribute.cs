using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Data.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DisplayStringAttribute : Attribute
    {
        private readonly string value;
        public string Value { get { return value; } }

        public string ResourceKey { get; set; }

        public DisplayStringAttribute() { }
        public DisplayStringAttribute(string v) { value = v; }
    }
}
