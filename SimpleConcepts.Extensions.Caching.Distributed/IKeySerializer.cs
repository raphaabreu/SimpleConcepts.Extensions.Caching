namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public interface IKeySerializer
    {
        string Serialize(object key);
    }
}