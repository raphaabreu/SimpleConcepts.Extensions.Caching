using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Caching.Distributed
{
    public static class MicrosoftDistributedCacheFallbackExtensions
    {

        public static byte[] Get(this IDistributedCache cache, string key, Func<byte[]> fallback, DistributedCacheEntryOptions options)
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

        public static string GetString(this IDistributedCache cache, string key, Func<string> fallback,
            DistributedCacheEntryOptions options)
        {
            var cached = cache.GetString(key);

            if (cached != null)
            {
                return cached;
            }

            var value = fallback();

            cache.SetString(key, value, options);

            return value;
        }

        public static async Task<string> GetStringAsync(this IDistributedCache cache, string key,
            Func<Task<string>> fallbackAsync, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var cached = await cache.GetStringAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await fallbackAsync();

            await cache.SetStringAsync(key, value, options, token);

            return value;
        }
    }
}