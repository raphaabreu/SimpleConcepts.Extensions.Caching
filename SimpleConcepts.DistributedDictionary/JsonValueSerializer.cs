using System;
using System.Text;
using System.Text.Json;

namespace SimpleConcepts.DistributedDictionary
{
    public class JsonValueSerializer : IValueSerializer
    {
        public JsonSerializerOptions Options { get; set; }

        public byte[] Serialize(object value)
        {
            var json = JsonSerializer.Serialize(value, Options);
            var bytes = Encoding.UTF8.GetBytes(json);

            return bytes;
        }

        public object Deserialize(byte[] bytes, Type type)
        {
            var json = Encoding.UTF8.GetString(bytes);
            var obj = JsonSerializer.Deserialize(json, type, Options);

            return obj;
        }
    }
}