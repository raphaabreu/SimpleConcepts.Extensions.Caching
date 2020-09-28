using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Caching.Distributed
{
    public static class MicrosoftDistributedCacheGetOrFetchExtensions
    {
        public static byte[] GetOrFetch(this IDistributedCache cache, string key, Func<byte[]> fetchCallback)
        {
            return cache.GetOrFetch(key, fetchCallback, new DistributedCacheEntryOptions());
        }

        public static byte[] GetOrFetch(this IDistributedCache cache, string key, Func<byte[]> fetchCallback,
            DistributedCacheEntryOptions options)
        {
            var cached = cache.Get(key);

            if (cached != null)
            {
                return cached;
            }

            var value = fetchCallback();

            cache.Set(key, value, options);

            return value;
        }

        public static Task<byte[]> GetOrFetchAsync(this IDistributedCache cache, string key, Func<Task<byte[]>> fetchCallback,
            CancellationToken token = default)
        {
            return cache.GetOrFetchAsync(key, fetchCallback, new DistributedCacheEntryOptions(), token);
        }

        public static async Task<byte[]> GetOrFetchAsync(this IDistributedCache cache, string key, Func<Task<byte[]>> fetchCallback,
            DistributedCacheEntryOptions options, CancellationToken token = default)
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

        public static string GetOrFetchString(this IDistributedCache cache, string key, Func<string> fetchCallback)
        {
            return cache.GetOrFetchString(key, fetchCallback, new DistributedCacheEntryOptions());
        }

        public static string GetOrFetchString(this IDistributedCache cache, string key, Func<string> fetchCallback,
            DistributedCacheEntryOptions options)
        {
            var cached = cache.GetString(key);

            if (cached != null)
            {
                return cached;
            }

            var x = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) };

            var value = fetchCallback();

            cache.SetString(key, value, options);

            return value;
        }

        public static Task<string> GetOrFetchStringAsync(this IDistributedCache cache, string key,
            Func<Task<string>> fetchCallback, CancellationToken token = default)
        {
            return cache.GetOrFetchStringAsync(key, fetchCallback, new DistributedCacheEntryOptions(), token);
        }

        public static async Task<string> GetOrFetchStringAsync(this IDistributedCache cache, string key,
            Func<Task<string>> fetchCallback, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var cached = await cache.GetStringAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fetchCallback();

            await cache.SetStringAsync(key, value, options, token);

            return value;
        }
    }
}