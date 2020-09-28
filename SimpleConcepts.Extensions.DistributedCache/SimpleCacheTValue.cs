using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCache<TValue> : ISimpleCache<TValue> where TValue : class
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

        public async Task<TValue?> GetAsync(CancellationToken token = default)
        {
            var bytes = await _cache.GetAsync(_key, token);

            return (TValue?)_valueSerializer.Deserialize(bytes, typeof(TValue));
        }

        public Task SetAsync(TValue value, CancellationToken token = default)
        {
            return SetAsync(value, _defaultEntryOptions, token);
        }

        public Task SetAsync(TValue value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var bytes = _valueSerializer.Serialize(value);

            return _cache.SetAsync(_key, bytes, options, token);
        }

        public Task RefreshAsync(CancellationToken token = default)
        {
            return _cache.RefreshAsync(_key, token);
        }

        public Task RemoveAsync(CancellationToken token = default)
        {
            return _cache.RemoveAsync(_key, token);
        }
    }
}