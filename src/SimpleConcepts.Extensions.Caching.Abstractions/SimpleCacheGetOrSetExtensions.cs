using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace SimpleConcepts.Extensions.Caching
{
    public static class SimpleCacheGetOrSetExtensions
    {
        public static async Task<TValue?> GetOrSetAsync<TValue>(this ISimpleCache<TValue> cache,
            Func<Task<TValue?>> valueFactory, CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(token);

            if (cached != null)
            {
                return cached;
            }

            var value = await valueFactory();

            if (value != null)
            {
                await cache.SetAsync(value, token);
            }

            return value;
        }

        public static async Task<TValue?> GetOrSetAsync<TValue>(this ISimpleCache<TValue> cache,
            Func<Task<TValue?>> valueFactory, DistributedCacheEntryOptions options,
            CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(token);

            if (cached != null)
            {
                return cached;
            }

            var value = await valueFactory();

            if (value != null)
            {
                await cache.SetAsync(value, options, token);
            }

            return value;
        }

        public static async Task<TValue?> GetOrSetAsync<TKey, TValue>(this ISimpleCache<TKey, TValue> cache, TKey key,
            Func<Task<TValue?>> valueFactory, CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await valueFactory();

            if (value != null)
            {
                await cache.SetAsync(key, value, token);
            }

            return value;
        }

        public static async Task<TValue?> GetOrSetAsync<TKey, TValue>(this ISimpleCache<TKey, TValue> cache, TKey key,
            Func<Task<TValue?>> valueFactory, DistributedCacheEntryOptions options,
            CancellationToken token = default) where TValue : class
        {
            var cached = await cache.GetAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await valueFactory();

            if (value != null)
            {
                await cache.SetAsync(key, value, options, token);
            }

            return value;
        }
    }
}