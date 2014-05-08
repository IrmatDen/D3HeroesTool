using Newtonsoft.Json;
using System;
using System.Linq;

namespace D3Data.Converters
{
    class HeroRefConverter : JsonConverter
    {
        public static Career careerRef { get; set; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(String.Format("Unexpected token parsing date. Expected Integer, got {0}.",
                                                  reader.TokenType));
            }

            var id = (long)reader.Value;
            return careerRef.heroes.First(h => h.id == id);
        }

        /// <summary>
        /// We won't write json, so no need to provide a meaningul implementation, right?
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HeroSummary);
        }
    }
}
