namespace SimpleConcepts.Extensions.Caching
{
    public interface IKeySerializer
    {
        string Serialize(object key);
    }
}