using Newtonsoft.Json;
using System.Collections.Generic;

namespace D3Data
{
    /// <summary>
    /// Provides D3 json -> D3Data bridges
    /// </summary>
    public class Deserializer
    {
        /// <summary>
        /// Converts (or try) some JSon into a Diablo 3 career
        /// </summary>
        /// <param name="json">json representing the career</param>
        /// <returns>newly loaded career</returns>
        public static Career AsCareer(string json)
        {
            Career career = new Career();
            Converters.HeroRefConverter.careerRef = career;
            JsonConvert.PopulateObject(json, career);

            return career;
        }

        /// <summary>
        /// Caches the list of hero we already loaded (to not re-populate them)
        /// </summary>
        private static HashSet<Hero> alreadyLoadedHeroes = new HashSet<Hero>();

        /// <summary>
        /// Finish loading the given hero based on provided JSon data (or no-op if the hero was already loaded)
        /// </summary>
        /// <param name="json">expected to contain the hero parameters</param>
        /// <param name="target">the hero which we want to finish loading</param>
        public static void CompleteHero(string json, Hero target)
        {
            if (alreadyLoadedHeroes.Contains(target))
                return;

            JsonConvert.PopulateObject(json, target);
            alreadyLoadedHeroes.Add(target);
        }
    }
}
