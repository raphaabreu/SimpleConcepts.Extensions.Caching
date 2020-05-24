namespace SimpleConcepts.DistributedDictionary
{
    public interface IKeySerializer
    {
        string Serialize(object key);
    }
}