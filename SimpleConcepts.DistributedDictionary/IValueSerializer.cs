using System;

namespace SimpleConcepts.DistributedDictionary
{
    public interface IValueSerializer
    {
        byte[] Serialize(object value);
        object Deserialize(byte[] bytes, Type type);
    }
}