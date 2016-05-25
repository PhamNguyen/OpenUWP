using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUWP.Converter
{
    public class LongToBoolJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(long));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return (long)reader.Value == 1;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
