namespace SimpleConcepts.DistributedDictionary
{
    public interface IDistributedDictionaryFactory
    {
        IDistributedDictionary<TKey, TValue> CreateDefaultDistributedDictionary<TKey, TValue>();
        IDistributedDictionary<TKey, TValue> CreateDistributedDictionary<TKey, TValue>(string name);
        IDistributedDictionary<TKey, TValue> CreateDistributedDictionary<TKey, TValue>();
    }
}