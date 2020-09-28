using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCache<TKey, TValue> : ISimpleCache<TKey, TValue> where TValue : class
    {
        private readonly IDistributedCache _cache;
        private readonly string _keySpace;
        private readonly IKeySerializer _keySerializer;
        private readonly IValueSerializer _valueSerializer;
        private readonly DistributedCacheEntryOptions _defaultEntryOptions;

        public SimpleCache(IDistributedCache cache, SimpleCacheOptions options)
        {
            _cache = cache;
            _keySpace = options.KeySpace ?? typeof(TValue).FullName + ":";
            _keySerializer = options.KeySerializer ?? new DefaultKeySerializer();
            _valueSerializer = options.ValueSerializer ?? new JsonValueSerializer();
            _defaultEntryOptions = options.DefaultEntryOptions ?? new DistributedCacheEntryOptions();
        }

        public async Task<TValue?> GetAsync(TKey key, CancellationToken token = default)
        {
            var bytes = await _cache.GetAsync(SerializeKey(key), token);

            return (TValue?)_valueSerializer.Deserialize(bytes, typeof(TValue));
        }

        public Task SetAsync(TKey key, TValue value, CancellationToken token = default)
        {
            return SetAsync(key, value, _defaultEntryOptions, token);
        }

        public Task SetAsync(TKey key, TValue value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var bytes = _valueSerializer.Serialize(value);

            return _cache.SetAsync(SerializeKey(key), bytes, options, token);
        }

        public Task RefreshAsync(TKey key, CancellationToken token = default)
        {
            return _cache.RefreshAsync(SerializeKey(key), token);
        }

        public Task RemoveAsync(TKey key, CancellationToken token = default)
        {
            return _cache.RemoveAsync(SerializeKey(key), token);
        }

        private string SerializeKey(TKey key)
        {
            return _keySpace + _keySerializer.Serialize(key!);
        }
    }
}