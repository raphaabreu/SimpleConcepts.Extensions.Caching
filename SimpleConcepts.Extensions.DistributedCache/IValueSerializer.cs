using System;

namespace SimpleConcepts.Extensions.Caching
{
    public interface IValueSerializer
    {
        byte[] Serialize(object value);
        object Deserialize(byte[] bytes, Type type);
    }
}