using Newtonsoft.Json;

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

        public static void CompleteHero(string json, HeroSummary target)
        {
            JsonConvert.PopulateObject(json, target);
        }
    }
}
