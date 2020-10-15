using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCache<TKey, TValue> : ISimpleCache<TKey, TValue> where TValue : class
    {
        private readonly IDistributedCache _cache;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _keySpace;
        private readonly IKeySerializer _keySerializer;
        private readonly IValueSerializer _valueSerializer;
        private readonly DistributedCacheEntryOptions _defaultEntryOptions;
        private readonly Func<TKey, IServiceProvider, CancellationToken, Task<TValue?>>? _valueFactory;
        private readonly bool _fallbackToFactoryOnException;

        public SimpleCache(IDistributedCache cache, IServiceProvider serviceProvider, SimpleCacheOptions<TKey, TValue> options)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
            _keySpace = options.KeySpace ?? typeof(TValue).FullName + ":";
            _keySerializer = options.KeySerializer ?? new DefaultKeySerializer();
            _valueSerializer = options.ValueSerializer ?? new JsonValueSerializer();
            _defaultEntryOptions = options.DefaultEntryOptions ?? new DistributedCacheEntryOptions();
            _valueFactory = options.ValueFactory;
            _fallbackToFactoryOnException = options.FallbackToFactoryOnException;
        }

        public async Task<TValue?> GetAsync(TKey key, CancellationToken token = default)
        {
            try
            {
                var bytes = await _cache.GetAsync(SerializeKey(key), token);
                var cached = (TValue?)_valueSerializer.Deserialize(bytes, typeof(TValue));

                if (cached != null)
                {
                    return cached;
                }
            }
            catch
            {
                if (_valueFactory == null || !_fallbackToFactoryOnException)
                {
                    throw;
                }
            }

            if (_valueFactory == null)
            {
                return null;
            }

            var value = await _valueFactory(key, _serviceProvider, token);

            if (value != null)
            {
                await SetAsync(key, value, token);
            }

            return value;
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