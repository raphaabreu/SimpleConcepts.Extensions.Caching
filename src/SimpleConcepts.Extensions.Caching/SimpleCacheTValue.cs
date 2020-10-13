using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public class SimpleCache<TValue> : ISimpleCache<TValue> where TValue : class
    {
        private readonly IDistributedCache _cache;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _key;
        private readonly IValueSerializer _valueSerializer;
        private readonly DistributedCacheEntryOptions _defaultEntryOptions;
        private readonly Func<IServiceProvider, CancellationToken, Task<TValue>>? _valueFactory;
        private readonly bool _fallbackToFactoryOnException;

        public SimpleCache(IDistributedCache cache, IServiceProvider serviceProvider, SimpleCacheOptions<TValue> options)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
            _key = options.KeySpace ?? typeof(TValue).FullName;
            _valueSerializer = options.ValueSerializer ?? new JsonValueSerializer();
            _defaultEntryOptions = options.DefaultEntryOptions ?? new DistributedCacheEntryOptions();
            _valueFactory = options.ValueFactory;
            _fallbackToFactoryOnException = options.FallbackToFactoryOnException;
        }

        public async Task<TValue?> GetAsync(CancellationToken token = default)
        {
            try
            {
                var bytes = await _cache.GetAsync(_key, token);
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

            var value = await _valueFactory(_serviceProvider, token);

            await SetAsync(value, token);

            return value;
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