using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace D3Data.Converters
{
    class UnixTimestampConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(String.Format("Unexpected token parsing date. Expected Integer, got {0}.",
                                                  reader.TokenType));
            }

            var timestamp = (long)reader.Value;
            DateTime dt = new DateTime(1970, 1, 1);
            return dt.AddSeconds(timestamp);
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
    }
}
