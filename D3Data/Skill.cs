using Newtonsoft.Json;
using System.Collections.Generic;

namespace D3Data
{
    /// <summary>
    /// Store properties common to both skills and runes
    /// </summary>
    public class SkillCommonProps
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    /// <summary>
    /// Describe a skill (active or passive)
    /// </summary>
    public class SkillDesc : SkillCommonProps
    {
        public string icon { get; set; }
    }


    /// <summary>
    /// Describe a rune (skill modificator, takes priority on skill rule)
    /// </summary>
    public class RuneDesc : SkillCommonProps
    {
    }

    /// <summary>
    /// Wrapper around a skill and rune combination
    /// </summary>
    public class Skill
    {
        public SkillDesc skill { get; set; }
        public RuneDesc rune { get; set; }
    }

    /// <summary>
    /// Represent the list of active and passive skills of a hero
    /// </summary>
    public class SkillSet
    {
        [JsonProperty(PropertyName = "active")]
        public List<Skill> actives { get; set; }

        [JsonProperty(PropertyName = "passive")]
        public List<Skill> passives { get; set; } 
    }
}
