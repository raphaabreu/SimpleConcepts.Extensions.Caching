using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace SimpleConcepts.DistributedDictionary
{
    public class DistributedDictionary<TKey, TValue> : IDistributedDictionary<TKey, TValue>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IKeySerializer _keySerializer;
        private readonly IValueSerializer _valueSerializer;
        private readonly string _keyNamespace;
        private readonly string _keyPrefix;

        public DistributedDictionary(IDistributedCache distributedCache, IOptions<DistributedDictionaryOptions> options)
        {
            _distributedCache = distributedCache;

            _keyNamespace = options.Value.KeyNamespace ?? Assembly.GetEntryAssembly()?.GetName().Name;
            _keyPrefix = options.Value.KeyPrefix ?? typeof(TValue).FullName;
            _keySerializer = options.Value.KeySerializer ?? new DefaultKeySerializer();
            _valueSerializer = options.Value.ValueSerializer ?? new JsonValueSerializer();
        }

        public async Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
        {
            var sKey = SerializeKey(key);
            var bytes = _valueSerializer.Serialize(value);

            await _distributedCache.SetAsync(sKey, bytes, cancellationToken);
        }

        public async Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var bytes = await _distributedCache.GetAsync(SerializeKey(key), cancellationToken);
            if (bytes == null)
            {
                throw new KeyNotFoundException();
            }

            return (TValue)_valueSerializer.Deserialize(bytes, typeof(TValue));
        }

        public async Task<bool> ContainsKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var bytes = await _distributedCache.GetAsync(SerializeKey(key), cancellationToken);

            return bytes != null;
        }

        public async Task<TValue> GetOrDefaultAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var bytes = await _distributedCache.GetAsync(SerializeKey(key), cancellationToken);
            if (bytes == null)
            {
                return default;
            }

            try
            {
                return (TValue)_valueSerializer.Deserialize(bytes, typeof(TValue));
            }
            catch (Exception)
            {
                return default;
            }
        }

        public Task RemoveAsync(TKey key, CancellationToken cancellationToken = default)
        {
            return _distributedCache.RemoveAsync(SerializeKey(key), cancellationToken);
        }

        private string SerializeKey(TKey key)
        {
            return $"{_keyNamespace}:{_keyPrefix}:{_keySerializer.Serialize(key)}";
        }
    }
}
