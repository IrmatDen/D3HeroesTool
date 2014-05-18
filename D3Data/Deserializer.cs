using Newtonsoft.Json;
using System.Collections.Generic;

namespace D3Data
{
    /// <summary>
    /// Provides D3 json -> D3Data bridges
    /// </summary>
    public class Deserializer
    {
        public static Career AsCareer(string json)
        {
            Career career = new Career();
            Converters.HeroRefConverter.careerRef = career;
            JsonConvert.PopulateObject(json, career);

            return career;
        }

        private static HashSet<Hero> alreadyLoadedHeroes = new HashSet<Hero>();
        public static void CompleteHero(string json, Hero target)
        {
            if (alreadyLoadedHeroes.Contains(target))
                return;

            JsonConvert.PopulateObject(json, target);
            alreadyLoadedHeroes.Add(target);
        }
    }
}
