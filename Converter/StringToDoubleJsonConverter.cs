using System;

using Newtonsoft.Json;
using OpenUWP.Utils;

namespace OpenUWP.Converter
{
    public class StringToDoubleJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(double));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader != null && reader.Value != null)
            {
                return ParseHelper.TryGetValue<double>(reader.Value.ToString());
            }
            return 0;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
