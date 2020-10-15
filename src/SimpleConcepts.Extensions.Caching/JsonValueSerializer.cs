using System;
using System.Text.Json;

namespace SimpleConcepts.Extensions.Caching
{
    public class JsonValueSerializer : IValueSerializer
    {
        public JsonSerializerOptions? Options { get; set; }

        public byte[] Serialize(object value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, Options);
        }

        public object? Deserialize(byte[]? bytes, Type type)
        {
            return bytes == null ? null : JsonSerializer.Deserialize(bytes, type, Options);
        }
    }
}