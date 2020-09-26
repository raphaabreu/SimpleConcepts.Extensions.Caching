﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public class DistributedCache<TKey, TValue> : IDistributedCache<TKey, TValue>
    {
        private readonly IDistributedCache _cache;
        private readonly string _keyPrefix;
        private readonly IKeySerializer _keySerializer;
        private readonly IValueSerializer _valueSerializer;
        private readonly DistributedCacheEntryOptions _defaultEntryOptions;

        public DistributedCache(IDistributedCache cache, IOptions<DistributedCacheOptions> options)
        {
            _cache = cache;
            _keyPrefix = options.Value.KeyPrefix ?? typeof(TValue).FullName + ":";
            _keySerializer = options.Value.KeySerializer ?? new DefaultKeySerializer();
            _valueSerializer = options.Value.ValueSerializer ?? new JsonValueSerializer();
            _defaultEntryOptions = options.Value.DefaultEntryOptions ?? new DistributedCacheEntryOptions();
        }

        public TValue Get(TKey key)
        {
            var bytes = _cache.Get(SerializeKey(key));

            return DeserializeValue(bytes);
        }

        public async Task<TValue> GetAsync(TKey key, CancellationToken token = default)
        {
            var bytes = await _cache.GetAsync(SerializeKey(key), token);

            return DeserializeValue(bytes);
        }

        public void Set(TKey key, TValue value, DistributedCacheEntryOptions options = default)
        {
            var bytes = SerializeValue(value);

            _cache.Set(SerializeKey(key), bytes, options ?? _defaultEntryOptions);
        }

        public Task SetAsync(TKey key, TValue value, DistributedCacheEntryOptions options = default,
            CancellationToken token = default)
        {
            var bytes = SerializeValue(value);

            return _cache.SetAsync(SerializeKey(key), bytes, options ?? _defaultEntryOptions, token);
        }

        public void Refresh(TKey key)
        {
            _cache.Refresh(SerializeKey(key));
        }

        public Task RefreshAsync(TKey key, CancellationToken token = default)
        {
            return _cache.RefreshAsync(SerializeKey(key), token);
        }

        public void Remove(TKey key)
        {
            _cache.Remove(SerializeKey(key));
        }

        public Task RemoveAsync(TKey key, CancellationToken token = default)
        {
            return _cache.RemoveAsync(SerializeKey(key), token);
        }

        private string SerializeKey(TKey key)
        {
            return _keyPrefix + _keySerializer.Serialize(key);
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