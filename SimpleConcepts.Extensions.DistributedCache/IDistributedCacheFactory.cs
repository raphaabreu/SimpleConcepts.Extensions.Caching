namespace SimpleConcepts.Extensions.Caching.Distributed
{
    internal interface IDistributedCacheFactory
    {
        IDistributedCache<TKey, TValue> CreateDefault<TKey, TValue>();
        IDistributedCache<TKey, TValue> Create<TKey, TValue>();
        IDistributedCache<TKey, TValue> Create<TKey, TValue>(string name);
    }
}