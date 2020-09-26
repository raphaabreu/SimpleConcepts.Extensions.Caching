using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public static class DistributedCacheFallbackExtensions
    {
        public static TValue Get<TKey, TValue>(this IDistributedCache<TKey, TValue> cache, TKey key,
            Func<TValue> fallback, DistributedCacheEntryOptions options = default) where TValue : class
        {
            var cached = cache.Get(key);

            if (cached != null)
            {
                return cached;
            }

            var value = fallback();

            cache.Set(key, value, options);

            return value;
        }

        public static async Task<TValue> GetAsync<TKey, TValue>(this IDistributedCache<TKey, TValue> cache, TKey key,
            Func<Task<TValue>> fallbackAsync, DistributedCacheEntryOptions options = default,
            CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fallbackAsync();

            await cache.SetAsync(key, value, options, token);

            return value;
        }
    }
}