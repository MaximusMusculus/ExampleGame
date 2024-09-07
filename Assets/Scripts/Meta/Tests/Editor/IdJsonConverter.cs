using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Application
{
    public class JsonConverterDtoId : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Id);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var strValue = (string) reader.Value;
                if (ushort.TryParse(strValue, out var value))
                {
                    return new Id(value);
                }

                throw new JsonSerializationException("Could not convert string to ushort.");
            }

            if (reader.TokenType == JsonToken.Integer)
            {
                var value = Convert.ToUInt16(reader.Value);
                return new Id(value);
            }

            throw new JsonSerializationException("Expected integer value.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int myInt = ((Id) value).Value;
            writer.WriteValue(myInt);
        }
    }
}