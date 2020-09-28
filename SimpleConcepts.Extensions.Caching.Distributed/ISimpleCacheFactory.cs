namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public interface ISimpleCacheFactory
    {
        ISimpleCache<TValue> Create<TValue>() where TValue : class;
        ISimpleCache<TValue> Create<TValue>(string name) where TValue : class;
        ISimpleCache<TKey, TValue> Create<TKey, TValue>() where TValue : class;
        ISimpleCache<TKey, TValue> Create<TKey, TValue>(string name) where TValue : class;
    }
}