using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace D3Data.Utils
{
    public static class Misc
    {
        /// <summary>
        /// Builds local path to image (requires getting the hero's class in a "bnet-io-friendly" format for WD/DH)
        /// thus exploiting reflection to get the serialization string defined in the enum
        /// </summary>
        /// <returns>local path to the background image for current hero</returns>
        public static string GetBackgroundNameForClass(D3Class desiredClass)
        {
            var classType = typeof(D3Class);
            var memberInfo = classType.GetMember(desiredClass.ToString());
            var attribs = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
            return attribs.Length > 0 ?
                  ((EnumMemberAttribute)attribs[0]).Value
                : desiredClass.ToString();
        }
    }
}
