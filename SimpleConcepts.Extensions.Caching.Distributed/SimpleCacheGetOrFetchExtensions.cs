using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching.Distributed
{
    public static class SimpleCacheGetOrFetchExtensions
    {
        public static async Task<TValue> GetOrFetchAsync<TValue>(this ISimpleCache<TValue> cache,
            Func<Task<TValue>> fetchCallback, CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fetchCallback();

            await cache.SetAsync(value, token);

            return value;
        }

        public static async Task<TValue> GetOrFetchAsync<TValue>(this ISimpleCache<TValue> cache,
            Func<Task<TValue>> fetchCallback, DistributedCacheEntryOptions options,
            CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fetchCallback();

            await cache.SetAsync(value, options, token);

            return value;
        }

        public static async Task<TValue> GetOrFetchAsync<TKey, TValue>(this ISimpleCache<TKey, TValue> cache, TKey key,
            Func<Task<TValue>> fetchCallback, CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fetchCallback();

            await cache.SetAsync(key, value, token);

            return value;
        }

        public static async Task<TValue> GetOrFetchAsync<TKey, TValue>(this ISimpleCache<TKey, TValue> cache, TKey key,
            Func<Task<TValue>> fetchCallback, DistributedCacheEntryOptions options,
            CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fetchCallback();

            await cache.SetAsync(key, value, options, token);

            return value;
        }
    }
}