using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCache<TValue> : ISimpleCache<TValue>
    {
        private readonly IDistributedCache _cache;
        private readonly string _key;
        private readonly IValueSerializer _valueSerializer;
        private readonly DistributedCacheEntryOptions _defaultEntryOptions;

        public SimpleCache(IDistributedCache cache, SimpleCacheOptions options)
        {
            _cache = cache;
            _key = options.KeySpace ?? typeof(TValue).FullName;
            _valueSerializer = options.ValueSerializer ?? new JsonValueSerializer();
            _defaultEntryOptions = options.DefaultEntryOptions ?? new DistributedCacheEntryOptions();
        }

        public TValue Get()
        {
            var bytes = _cache.Get(_key);

            return DeserializeValue(bytes);
        }

        public async Task<TValue> GetAsync(CancellationToken token = default)
        {
            var bytes = await _cache.GetAsync(_key, token);

            return DeserializeValue(bytes);
        }

        public void Set(TValue value, DistributedCacheEntryOptions options = default)
        {
            var bytes = SerializeValue(value);

            _cache.Set(_key, bytes, options ?? _defaultEntryOptions);
        }

        public Task SetAsync(TValue value, DistributedCacheEntryOptions options = default,
            CancellationToken token = default)
        {
            var bytes = SerializeValue(value);

            return _cache.SetAsync(_key, bytes, options ?? _defaultEntryOptions, token);
        }

        public void Refresh()
        {
            _cache.Refresh(_key);
        }

        public Task RefreshAsync(CancellationToken token = default)
        {
            return _cache.RefreshAsync(_key, token);
        }

        public void Remove()
        {
            _cache.Remove(_key);
        }

        public Task RemoveAsync(CancellationToken token = default)
        {
            return _cache.RemoveAsync(_key, token);
        }

        private byte[] SerializeValue(TValue value)
        {
            return _valueSerializer.Serialize(value);
        }

        private TValue DeserializeValue(byte[] value)
        {
            return (TValue)_valueSerializer.Deserialize(value, typeof(TValue));
        }
    }
}