using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheGetOrSetExtensions
    {
        public static void Set(this IDistributedCache cache, string key, byte[] value, TimeSpan absoluteExpirationRelativeToNow)
        {
            cache.Set(key, value,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow });
        }

        public static Task SetAsync(this IDistributedCache cache, string key, byte[] value, TimeSpan absoluteExpirationRelativeToNow, CancellationToken token = default)
        {
            return cache.SetAsync(key, value,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow }, token);
        }


        public static byte[]? GetOrSet(this IDistributedCache cache, string key, Func<byte[]?> valueFactory)
        {
            return cache.GetOrSet(key, valueFactory, new DistributedCacheEntryOptions());
        }

        public static byte[]? GetOrSet(this IDistributedCache cache, string key, Func<byte[]?> valueFactory,
            TimeSpan absoluteExpirationRelativeToNow)
        {
            return cache.GetOrSet(key, valueFactory,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow });
        }

        public static byte[]? GetOrSet(this IDistributedCache cache, string key, Func<byte[]?> valueFactory,
            DistributedCacheEntryOptions options)
        {
            var cached = cache.Get(key);

            if (cached != null)
            {
                return cached;
            }

            var value = valueFactory();

            if (value != null)
            {
                cache.Set(key, value, options);
            }

            return value;
        }


        public static Task<byte[]?> GetOrSetAsync(this IDistributedCache cache, string key, Func<Task<byte[]?>> valueFactory,
            CancellationToken token = default)
        {
            return cache.GetOrSetAsync(key, valueFactory, new DistributedCacheEntryOptions(), token);
        }

        public static Task<byte[]?> GetOrSetAsync(this IDistributedCache cache, string key, Func<Task<byte[]?>> valueFactory,
            TimeSpan absoluteExpirationRelativeToNow, CancellationToken token = default)
        {
            return cache.GetOrSetAsync(key, valueFactory,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow }, token);
        }

        public static async Task<byte[]?> GetOrSetAsync(this IDistributedCache cache, string key, Func<Task<byte[]?>> valueFactory,
            DistributedCacheEntryOptions options, CancellationToken token = default)
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


        public static string? GetOrSetString(this IDistributedCache cache, string key, Func<string?> valueFactory)
        {
            return cache.GetOrSetString(key, valueFactory, new DistributedCacheEntryOptions());
        }

        public static string? GetOrSetString(this IDistributedCache cache, string key, Func<string?> valueFactory,
            TimeSpan absoluteExpirationRelativeToNow)
        {
            return cache.GetOrSetString(key, valueFactory,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow });
        }

        public static string? GetOrSetString(this IDistributedCache cache, string key, Func<string?> valueFactory,
            DistributedCacheEntryOptions options)
        {
            var cached = cache.GetString(key);

            if (cached != null)
            {
                return cached;
            }

            var value = valueFactory();

            if (value != null)
            {
                cache.SetString(key, value, options);
            }

            return value;
        }


        public static Task<string?> GetOrSetStringAsync(this IDistributedCache cache, string key,
            Func<Task<string?>> valueFactory, CancellationToken token = default)
        {
            return cache.GetOrSetStringAsync(key, valueFactory, new DistributedCacheEntryOptions(), token);
        }

        public static Task<string?> GetOrSetStringAsync(this IDistributedCache cache, string key,
            Func<Task<string?>> valueFactory, TimeSpan absoluteExpirationRelativeToNow, CancellationToken token = default)
        {
            return cache.GetOrSetStringAsync(key, valueFactory,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow }, token);
        }

        public static async Task<string?> GetOrSetStringAsync(this IDistributedCache cache, string key,
            Func<Task<string?>> valueFactory, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var cached = await cache.GetStringAsync(key, token);

            if (cached != null)
            {
                return cached;
            }

            var value = await valueFactory();

            if (value != null)
            {
                await cache.SetStringAsync(key, value, options, token);
            }

            return value;
        }
    }
}