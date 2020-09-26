namespace SimpleConcepts.Extensions.Caching
{
    public interface ISimpleCacheFactory
    {
        ISimpleCache<TValue> Create<TValue>();
        ISimpleCache<TValue> Create<TValue>(string name);
        ISimpleCache<TKey, TValue> Create<TKey, TValue>();
        ISimpleCache<TKey, TValue> Create<TKey, TValue>(string name);
    }
}